namespace Atata;

/// <summary>
/// Represents the log manager, an entry point for the Atata logging functionality.
/// </summary>
/// <seealso cref="ILogManager" />
public class LogManager : ILogManager
{
    private readonly ILogEventInfoFactory _logEventInfoFactory;

    private readonly List<LogConsumerConfiguration> _logConsumerConfigurations = [];

    private readonly List<SecretStringToMask> _secretStringsToMask = [];

    private readonly Stack<LogSection> _sectionEndStack = new();

    public LogManager(ILogEventInfoFactory logEventInfoFactory) =>
        _logEventInfoFactory = logEventInfoFactory.CheckNotNull(nameof(logEventInfoFactory));

    /// <summary>
    /// Use the specified consumer configuration for logging.
    /// </summary>
    /// <param name="logConsumerConfiguration">The log consumer configuration.</param>
    /// <returns>
    /// The same <see cref="LogManager" /> instance.
    /// </returns>
    public LogManager Use(LogConsumerConfiguration logConsumerConfiguration)
    {
        logConsumerConfiguration.CheckNotNull(nameof(logConsumerConfiguration));

        _logConsumerConfigurations.Add(logConsumerConfiguration);

        return this;
    }

    /// <summary>
    /// Adds the secret strings to mask.
    /// </summary>
    /// <param name="secretStringsToMask">The secret strings to mask.</param>
    /// <returns>The same <see cref="LogManager"/> instance.</returns>
    public LogManager AddSecretStringsToMask(IEnumerable<SecretStringToMask> secretStringsToMask)
    {
        secretStringsToMask.CheckNotNull(nameof(secretStringsToMask));

        _secretStringsToMask.AddRange(secretStringsToMask);

        return this;
    }

    /// <inheritdoc/>
    public void Trace(string message) =>
        Log(LogLevel.Trace, message);

    /// <inheritdoc/>
    public void Debug(string message) =>
        Log(LogLevel.Debug, message);

    /// <inheritdoc/>
    public void Info(string message) =>
        Log(LogLevel.Info, message);

    /// <inheritdoc/>
    public void Warn(string message) =>
        Log(LogLevel.Warn, message);

    /// <inheritdoc/>
    public void Warn(Exception exception) =>
        Log(LogLevel.Warn, null, exception);

    /// <inheritdoc/>
    public void Warn(Exception exception, string message) =>
        Log(LogLevel.Warn, message, exception);

    /// <inheritdoc/>
    public void Error(Exception exception) =>
        Log(LogLevel.Error, null, exception);

    /// <inheritdoc/>
    public void Error(string message) =>
        Log(LogLevel.Error, message);

    /// <inheritdoc/>
    public void Error(Exception exception, string message) =>
        Log(LogLevel.Error, message, exception);

    /// <inheritdoc/>
    public void Fatal(Exception exception) =>
        Log(LogLevel.Fatal, null, exception);

    /// <inheritdoc/>
    public void Fatal(string message) =>
        Log(LogLevel.Fatal, message);

    /// <inheritdoc/>
    public void Fatal(Exception exception, string message) =>
        Log(LogLevel.Fatal, message, exception);

    /// <inheritdoc/>
    public void ExecuteSection(LogSection section, Action action)
    {
        section.CheckNotNull(nameof(section));
        action.CheckNotNull(nameof(action));

        StartSection(section);

        try
        {
            action?.Invoke();
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

    /// <inheritdoc/>
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

            message += message[message.Length - 1] == '.'
                ? ".."
                : "...";
        }

        return $"{exception.GetType().FullName}: {message}";
    }

    private static string PrependHierarchyPrefixesToMessage(string message, LogEventInfo eventInfo, LogConsumerConfiguration logConsumerConfiguration)
    {
        StringBuilder builder = new StringBuilder();

        if (eventInfo.NestingLevel > 0)
        {
            for (int i = 0; i < eventInfo.NestingLevel; i++)
            {
                builder.Append(logConsumerConfiguration.MessageNestingLevelIndent);
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
                        ? logConsumerConfiguration.MessageStartSectionPrefix
                        : logConsumerConfiguration.MessageEndSectionPrefix);
            }
        }

        string resultMessage = builder.Append(message).ToString();

        return resultMessage.Length == 0 && message == null
            ? null
            : resultMessage;
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

    private void StartSection(LogSection section)
    {
        LogEventInfo eventInfo = _logEventInfoFactory.Create(section.Level, section.Message);
        eventInfo.SectionStart = section;

        section.StartedAt = eventInfo.Timestamp;
        section.Stopwatch.Start();

        Log(eventInfo);

        _sectionEndStack.Push(section);
    }

    private void EndCurrentSection()
    {
        if (_sectionEndStack.Any())
        {
            LogSection section = _sectionEndStack.Pop();

            section.Stopwatch.Stop();

            string message = $"{section.Message} ({section.ElapsedTime.ToLongIntervalString()})";

            if (section.IsResultSet)
                message = AppendSectionResultToMessage(message, section.Result);
            else if (section.Exception != null)
                message = AppendSectionResultToMessage(message, section.Exception);

            LogEventInfo eventInfo = _logEventInfoFactory.Create(section.Level, message);
            eventInfo.SectionEnd = section;

            Log(eventInfo);
        }
    }

    private void Log(LogLevel level, string message, Exception exception = null)
    {
        LogEventInfo logEvent = _logEventInfoFactory.Create(level, message);
        logEvent.Exception = exception;

        Log(logEvent);
    }

    private void Log(LogEventInfo eventInfo)
    {
        var appropriateConsumerItems = _logConsumerConfigurations
            .Where(x => eventInfo.Level >= x.MinLevel);

        if (eventInfo.SectionEnd != null)
            appropriateConsumerItems = FilterByLogSectionEnd(appropriateConsumerItems, eventInfo.SectionEnd);

        string originalMessage = ApplySecretMasks(eventInfo.Message);

        foreach (var consumerItem in appropriateConsumerItems)
        {
            eventInfo.NestingLevel = _sectionEndStack.Count(x => x.Level >= consumerItem.MinLevel);
            eventInfo.Message = PrependHierarchyPrefixesToMessage(originalMessage, eventInfo, consumerItem);

            consumerItem.Consumer.Log(eventInfo);
        }
    }

    private string ApplySecretMasks(string message)
    {
        foreach (var secret in _secretStringsToMask)
            message = message.Replace(secret.Value, secret.Mask);

        return message;
    }
}
