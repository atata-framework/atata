using SeleniumLogLevel = OpenQA.Selenium.LogLevel;

namespace Atata;

/// <summary>
/// Represents the browser log entry.
/// </summary>
public sealed class BrowserLogEntry
{
    private BrowserLogEntry(DateTime utcTimestamp, LogLevel level, string message)
    {
        UtcTimestamp = utcTimestamp;
        Level = level;
        Message = message;
    }

    /// <summary>
    /// Gets the timestamp in UTC form.
    /// </summary>
    public DateTime UtcTimestamp { get; }

    [Obsolete("Use UtcTimestamp instead.")] // Obsolete since v4.0.0.
    public DateTime Timestamp => UtcTimestamp;

    /// <summary>
    /// Gets the level of log entry.
    /// </summary>
    public LogLevel Level { get; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message { get; }

    internal static BrowserLogEntry Create(LogEntry logEntry) =>
        new(
            logEntry.Timestamp,
            ConvertSeleniumLogLevel(logEntry.Level),
            CorrectLineBreaksInMessage(logEntry.Message));

    private static LogLevel ConvertSeleniumLogLevel(SeleniumLogLevel logLevel) =>
        logLevel switch
        {
            SeleniumLogLevel.All => LogLevel.Trace,
            SeleniumLogLevel.Debug => LogLevel.Debug,
            SeleniumLogLevel.Info => LogLevel.Info,
            SeleniumLogLevel.Warning => LogLevel.Warn,
            SeleniumLogLevel.Severe => LogLevel.Error,
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(logLevel)
        };

    private static string CorrectLineBreaksInMessage(string message) =>
        message.Replace("\\n", "\n");
}
