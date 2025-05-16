namespace Atata;

/// <summary>
/// Represents the log manager, an entry point for the Atata logging functionality.
/// </summary>
internal sealed class LogManager : ILogManager
{
    private readonly LogManagerConfiguration _configuration;

    private readonly ILogEventInfoFactory _logEventInfoFactory;

    private readonly Lazy<ConcurrentDictionary<string, ILogManager>> _lazySourceLogManagerMap = new();

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

    public void Log(LogLevel level, string? message, Exception? exception = null) =>
        Log(
            DateTime.UtcNow,
            level,
            message,
            exception);

    public void Log(DateTime utcTimestamp, LogLevel level, string? message, Exception? exception = null)
    {
        DateTime logTimestamp = AtataContext.GlobalProperties.ConvertToTimeZone(utcTimestamp);
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
        Guard.ThrowIfNull(section);
        Guard.ThrowIfNull(action);

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
        Guard.ThrowIfNull(section);
        Guard.ThrowIfNull(function);

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
        Guard.ThrowIfNull(section);
        Guard.ThrowIfNull(function);

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
        Guard.ThrowIfNull(section);
        Guard.ThrowIfNull(function);

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
            CreateDynamicNestingLevelResolver(x => x.EmbedSessionLog));

    public ILogManager ForSource(string source)
    {
        Guard.ThrowIfNullOrWhitespace(source);

        return _lazySourceLogManagerMap.Value.GetOrAdd(
            source,
            x => new LogManager(
                _configuration,
                new SourceLogEventInfoFactory(_logEventInfoFactory, x),
                CreateDynamicNestingLevelResolver(x => x.EmbedSourceLog)));
    }

    public ILogManager ForCategory(string category)
    {
        Guard.ThrowIfNullOrWhitespace(category);

        return _lazyCategoryLogManagerMap.Value.GetOrAdd(
            category,
            x => new LogManager(
                _configuration,
                new CategoryLogEventInfoFactory(_logEventInfoFactory, x),
                CreateDynamicNestingLevelResolver(_ => true)));
    }

    public ILogManager ForCategory<TCategory>() =>
        ForCategory(typeof(TCategory).FullName);

    public ILogManager CreateSubLog()
    {
        LogSection[] sectionsSpanshot = GetSectionsSnapshot();

        return new LogManager(
            _configuration,
            _logEventInfoFactory,
            new FixedOuterLogNestingLevelResolver(sectionsSpanshot, _outerLogNestingLevelResolver));
    }

    public ILogManager CreateSubLogForCategory(string category)
    {
        Guard.ThrowIfNullOrWhitespace(category);

        LogSection[] sectionsSpanshot = GetSectionsSnapshot();

        return new LogManager(
            _configuration,
            new CategoryLogEventInfoFactory(_logEventInfoFactory, category),
            new FixedOuterLogNestingLevelResolver(sectionsSpanshot, _outerLogNestingLevelResolver));
    }

    private static DateTime GetCurrentTimestamp() =>
        AtataContext.GlobalProperties.ConvertToTimeZone(DateTime.UtcNow);

    private static string AppendSectionResultToMessage(string message, string resultAsString)
    {
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
            message = message[..newLineIndex];

            message += message[^1] == '.'
                ? ".."
                : "...";
        }

        return $"{exception.GetType().FullName}: {message}";
    }

    private static string? BuildNestingText(LogEventInfo eventInfo, LogConsumerConfiguration logConsumerConfiguration)
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

    private static void HandleExceptionOccurredDuringLogging(Exception exception)
    {
        try
        {
            Console.WriteLine(exception.ToString());
        }
        catch
        {
            // Do nothing. Seems like the console is not available, so nowhere to log.
        }
    }

    private DynamicOuterLogNestingLevelResolver CreateDynamicNestingLevelResolver(
        Func<LogConsumerConfiguration, bool> isEnabledPredicate)
        =>
        new(_sectionStack, _outerLogNestingLevelResolver, isEnabledPredicate);

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
            if (_sectionStack.Count > 0)
            {
                LogSection section = _sectionStack.Pop();

                section.Stopwatch.Stop();

                string message = $"{section.Message} ({section.ElapsedTime.ToLongIntervalString()})";

                if (section.IsResultSet && section.LogResult)
                    message = AppendSectionResultToMessage(message, Stringifier.ToString(section.Result));
                else if (section.Exception is not null)
                    message = AppendSectionResultToMessage(message, BuildExceptionShortSingleLineMessage(section.Exception));

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

        eventInfo.Message = eventInfo.Message is null
            ? null
            : ApplySecretMasks(eventInfo.Message);

        lock (_sectionStack)
        {
            foreach (var consumerItem in appropriateConsumerItems)
            {
                int outerNestingLevel = _outerLogNestingLevelResolver.GetNestingLevel(consumerItem);
                int thisNestingLevel = _sectionStack.Count(x => x.Level >= consumerItem.MinLevel);

                eventInfo.NestingLevel = outerNestingLevel + thisNestingLevel;
                eventInfo.NestingText = BuildNestingText(eventInfo, consumerItem);

                try
                {
                    consumerItem.Consumer.Log(eventInfo);
                }
                catch (Exception exception)
                {
                    HandleExceptionOccurredDuringLogging(exception);
                }
            }
        }
    }

    private LogSection[] GetSectionsSnapshot()
    {
        lock (_sectionStack)
        {
            return [.. _sectionStack];
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

    private sealed class DynamicOuterLogNestingLevelResolver : IOuterLogNestingLevelResolver
    {
        private readonly IEnumerable<LogSection> _sections;

        private readonly IOuterLogNestingLevelResolver _parentResolver;

        private readonly Func<LogConsumerConfiguration, bool> _isEnabledPredicate;

        public DynamicOuterLogNestingLevelResolver(
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

    private sealed class FixedOuterLogNestingLevelResolver : IOuterLogNestingLevelResolver
    {
        private readonly IReadOnlyCollection<LogSection> _sections;

        private readonly IOuterLogNestingLevelResolver _parentResolver;

        public FixedOuterLogNestingLevelResolver(
            IReadOnlyCollection<LogSection> sections,
            IOuterLogNestingLevelResolver parentResolver)
        {
            _sections = sections;
            _parentResolver = parentResolver;
        }

        public int GetNestingLevel(LogConsumerConfiguration consumerConfiguration)
        {
            int parentNestingLevel = _parentResolver.GetNestingLevel(consumerConfiguration);

            return parentNestingLevel + _sections.Count(x => x.Level >= consumerConfiguration.MinLevel);
        }
    }
}
