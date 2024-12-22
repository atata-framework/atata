namespace Atata;

/// <summary>
/// Represents the log manager, an entry point for the Atata logging functionality.
/// </summary>
/// <seealso cref="ILogManager" />
internal sealed class LogManager : ILogManager
{
    private readonly LogManagerConfiguration _configuration;

    private readonly ILogEventInfoFactory _logEventInfoFactory;

    private readonly Lazy<ConcurrentDictionary<string, ILogManager>> _lazyExternalSourceLogManagerMap = new();

    private readonly Lazy<ConcurrentDictionary<string, ILogManager>> _lazyCategoryLogManagerMap = new();

    private readonly IOuterLogNestingLevelResolver _outerLogNestingLevelResolver;

    private readonly Stack<LogSection> _sectionStack = new();

    internal LogManager(
        LogManagerConfiguration configuration,
        ILogEventInfoFactory logEventInfoFactory)
        : this(
            configuration,
            logEventInfoFactory,
            ZeroOuterLogNestingLevelResolver.Instance)
    {
    }

    private LogManager(
        LogManagerConfiguration configuration,
        ILogEventInfoFactory logEventInfoFactory,
        IOuterLogNestingLevelResolver outerLogNestingLevelResolver)
    {
        _configuration = configuration;
        _logEventInfoFactory = logEventInfoFactory;
        _outerLogNestingLevelResolver = outerLogNestingLevelResolver;
    }

    private interface IOuterLogNestingLevelResolver
    {
        int GetNestingLevel(LogConsumerConfiguration consumerConfiguration);
    }

    public void Log(LogLevel level, string message, Exception exception = null) =>
        Log(
            DateTime.UtcNow,
            level,
            message,
            exception);

    public void Log(DateTime utcTimestamp, LogLevel level, string message, Exception exception = null)
    {
        DateTime logTimestamp = ConvertUtcTimestampToAtataTimeZone(utcTimestamp);
        LogEventInfo logEvent = _logEventInfoFactory.Create(logTimestamp, level, message);
        logEvent.Exception = exception;

        Log(logEvent);
    }

    public void Trace(string message) =>
        Log(LogLevel.Trace, message);

    public void Debug(string message) =>
        Log(LogLevel.Debug, message);

    public void Info(string message) =>
        Log(LogLevel.Info, message);

    public void Warn(string message) =>
        Log(LogLevel.Warn, message);

    public void Warn(Exception exception) =>
        Log(LogLevel.Warn, null, exception);

    public void Warn(Exception exception, string message) =>
        Log(LogLevel.Warn, message, exception);

    public void Error(Exception exception) =>
        Log(LogLevel.Error, null, exception);

    public void Error(string message) =>
        Log(LogLevel.Error, message);

    public void Error(Exception exception, string message) =>
        Log(LogLevel.Error, message, exception);

    public void Fatal(Exception exception) =>
        Log(LogLevel.Fatal, null, exception);

    public void Fatal(string message) =>
        Log(LogLevel.Fatal, message);

    public void Fatal(Exception exception, string message) =>
        Log(LogLevel.Fatal, message, exception);

    public void ExecuteSection(LogSection section, Action action)
    {
        section.CheckNotNull(nameof(section));
        action.CheckNotNull(nameof(action));

        StartSection(section);

        try
        {
            action.Invoke();
        }
        catch (Exception exception)
        {
            section.Exception = exception;
            throw;
        }
        finally
        {
            EndCurrentSection();
        }
    }

    public TResult ExecuteSection<TResult>(LogSection section, Func<TResult> function)
    {
        section.CheckNotNull(nameof(section));
        function.CheckNotNull(nameof(function));

        StartSection(section);

        try
        {
            TResult result = function.Invoke();
            section.Result = result;
            return result;
        }
        catch (Exception exception)
        {
            section.Exception = exception;
            throw;
        }
        finally
        {
            EndCurrentSection();
        }
    }

    public async Task ExecuteSectionAsync(LogSection section, Func<Task> function)
    {
        section.CheckNotNull(nameof(section));
        function.CheckNotNull(nameof(function));

        StartSection(section);

        try
        {
            await function.Invoke().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            section.Exception = exception;
            throw;
        }
        finally
        {
            EndCurrentSection();
        }
    }

    public async Task<TResult> ExecuteSectionAsync<TResult>(LogSection section, Func<Task<TResult>> function)
    {
        section.CheckNotNull(nameof(section));
        function.CheckNotNull(nameof(function));

        StartSection(section);

        try
        {
            TResult result = await function.Invoke().ConfigureAwait(false);
            section.Result = result;
            return result;
        }
        catch (Exception exception)
        {
            section.Exception = exception;
            throw;
        }
        finally
        {
            EndCurrentSection();
        }
    }

    internal ILogManager ForSession(AtataSession session) =>
        new LogManager(
            _configuration,
            new AtataSessionLogEventInfoFactory(_logEventInfoFactory, session),
            CreateNestingLevelResolver(x => x.EmbedSessionLog));

    public ILogManager ForExternalSource(string externalSource)
    {
        externalSource.CheckNotNullOrWhitespace(nameof(externalSource));

        return _lazyExternalSourceLogManagerMap.Value.GetOrAdd(
            externalSource,
            x => new LogManager(
                _configuration,
                new ExternalSourceLogEventInfoFactory(_logEventInfoFactory, x),
                CreateNestingLevelResolver(x => x.EmbedExternalSourceLog)));
    }

    public ILogManager ForCategory(string category)
    {
        category.CheckNotNullOrWhitespace(nameof(category));

        return _lazyCategoryLogManagerMap.Value.GetOrAdd(
            category,
            x => new LogManager(
                _configuration,
                new CategoryLogEventInfoFactory(_logEventInfoFactory, x),
                CreateNestingLevelResolver(_ => true)));
    }

    public ILogManager ForCategory<TCategory>() =>
        ForCategory(typeof(TCategory).FullName);

    private static DateTime GetCurrentTimestamp() =>
        ConvertUtcTimestampToAtataTimeZone(DateTime.UtcNow);

    private static DateTime ConvertUtcTimestampToAtataTimeZone(DateTime utcTimestamp) =>
        TimeZoneInfo.ConvertTimeFromUtc(utcTimestamp, AtataContext.GlobalProperties.TimeZone);

    private static string AppendSectionResultToMessage(string message, object result)
    {
        string resultAsString = result is Exception resultAsException
            ? BuildExceptionShortSingleLineMessage(resultAsException)
            : Stringifier.ToString(result);

        string separator = resultAsString.Contains(Environment.NewLine)
            ? Environment.NewLine
            : " ";

        return $"{message} >>{separator}{resultAsString}";
    }

    private static string BuildExceptionShortSingleLineMessage(Exception exception)
    {
        string message = exception.Message;

        int newLineIndex = message.IndexOf(Environment.NewLine, StringComparison.Ordinal);

        if (newLineIndex >= 0)
        {
            message = message.Substring(0, newLineIndex);

            message += message[^1] == '.'
                ? ".."
                : "...";
        }

        return $"{exception.GetType().FullName}: {message}";
    }

    private static string BuildNestingText(LogEventInfo eventInfo, LogConsumerConfiguration logConsumerConfiguration)
    {
        StringBuilder builder = new();

        if (eventInfo.NestingLevel > 0)
        {
            for (int i = 0; i < eventInfo.NestingLevel; i++)
            {
                builder.Append(logConsumerConfiguration.NestingLevelIndent);
            }
        }

        if (logConsumerConfiguration.SectionEnd != LogSectionEndOption.Exclude)
        {
            var logSection = eventInfo.SectionStart ?? eventInfo.SectionEnd;

            if (logSection is not null &&
                (logConsumerConfiguration.SectionEnd == LogSectionEndOption.Include || IsBlockLogSection(logSection)))
            {
                builder.Append(
                    eventInfo.SectionStart != null
                        ? logConsumerConfiguration.SectionStartPrefix
                        : logConsumerConfiguration.SectionEndPrefix);
            }
        }

        return builder.Length > 0
            ? builder.ToString()
            : null;
    }

    private static IEnumerable<LogConsumerConfiguration> FilterByLogSectionEnd(
        IEnumerable<LogConsumerConfiguration> configurations,
        LogSection logSection)
    {
        foreach (var configuration in configurations)
        {
            if (configuration.SectionEnd == LogSectionEndOption.Include ||
                (configuration.SectionEnd == LogSectionEndOption.IncludeForBlocks && IsBlockLogSection(logSection)))
                yield return configuration;
        }
    }

    private static bool IsBlockLogSection(LogSection logSection) =>
        logSection is AggregateAssertionLogSection or SetupLogSection or StepLogSection;

    private IOuterLogNestingLevelResolver CreateNestingLevelResolver(
        Func<LogConsumerConfiguration, bool> isEnabledPredicate)
        =>
        new OuterLogNestingLevelResolver(
            _sectionStack, _outerLogNestingLevelResolver, isEnabledPredicate);

    private void StartSection(LogSection section)
    {
        lock (_sectionStack)
        {
            LogEventInfo eventInfo = _logEventInfoFactory.Create(GetCurrentTimestamp(), section.Level, section.Message);
            eventInfo.SectionStart = section;

            section.StartedAt = eventInfo.Timestamp;
            section.Stopwatch.Start();

            Log(eventInfo);

            _sectionStack.Push(section);
        }
    }

    private void EndCurrentSection()
    {
        lock (_sectionStack)
        {
            if (_sectionStack.Any())
            {
                LogSection section = _sectionStack.Pop();

                section.Stopwatch.Stop();

                string message = $"{section.Message} ({section.ElapsedTime.ToLongIntervalString()})";

                if (section.IsResultSet)
                    message = AppendSectionResultToMessage(message, section.Result);
                else if (section.Exception != null)
                    message = AppendSectionResultToMessage(message, section.Exception);

                LogEventInfo eventInfo = _logEventInfoFactory.Create(GetCurrentTimestamp(), section.Level, message);
                eventInfo.SectionEnd = section;

                Log(eventInfo);
            }
        }
    }

    private void Log(LogEventInfo eventInfo)
    {
        var appropriateConsumerItems = _configuration.ConsumerConfigurations
            .Where(x => eventInfo.Level >= x.MinLevel);

        if (eventInfo.SectionEnd != null)
            appropriateConsumerItems = FilterByLogSectionEnd(appropriateConsumerItems, eventInfo.SectionEnd);

        eventInfo.Message = ApplySecretMasks(eventInfo.Message);

        lock (_sectionStack)
        {
            foreach (var consumerItem in appropriateConsumerItems)
            {
                var outerNestingLevel = _outerLogNestingLevelResolver.GetNestingLevel(consumerItem);
                var thisNestingLevel = _sectionStack.Count(x => x.Level >= consumerItem.MinLevel);

                eventInfo.NestingLevel = outerNestingLevel + thisNestingLevel;
                eventInfo.NestingText = BuildNestingText(eventInfo, consumerItem);

                consumerItem.Consumer.Log(eventInfo);
            }
        }
    }

    private string ApplySecretMasks(string message)
    {
        foreach (var secret in _configuration.SecretStringsToMask)
            message = message.Replace(secret.Value, secret.Mask);

        return message;
    }

    private sealed class ZeroOuterLogNestingLevelResolver : IOuterLogNestingLevelResolver
    {
        public static ZeroOuterLogNestingLevelResolver Instance { get; } = new();

        public int GetNestingLevel(LogConsumerConfiguration consumerConfiguration) => 0;
    }

    private sealed class OuterLogNestingLevelResolver : IOuterLogNestingLevelResolver
    {
        private readonly IEnumerable<LogSection> _sections;

        private readonly IOuterLogNestingLevelResolver _parentResolver;

        private readonly Func<LogConsumerConfiguration, bool> _isEnabledPredicate;

        public OuterLogNestingLevelResolver(
            IEnumerable<LogSection> sections,
            IOuterLogNestingLevelResolver parentResolver,
            Func<LogConsumerConfiguration, bool> isEnabledPredicate)
        {
            _sections = sections;
            _parentResolver = parentResolver;
            _isEnabledPredicate = isEnabledPredicate;
        }

        public int GetNestingLevel(LogConsumerConfiguration consumerConfiguration)
        {
            if (!_isEnabledPredicate.Invoke(consumerConfiguration))
                return 0;

            int parentNestingLevel = _parentResolver.GetNestingLevel(consumerConfiguration);

            lock (_sections)
            {
                return parentNestingLevel + _sections.Count(x => x.Level >= consumerConfiguration.MinLevel);
            }
        }
    }
}
