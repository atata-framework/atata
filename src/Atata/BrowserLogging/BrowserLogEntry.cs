using SeleniumLogLevel = OpenQA.Selenium.LogLevel;

namespace Atata;

/// <summary>
/// Represents the browser log entry.
/// </summary>
public sealed class BrowserLogEntry
{
    private BrowserLogEntry(DateTime timestamp, LogLevel level, string message)
    {
        Timestamp = timestamp;
        Level = level;
        Message = message;
    }

    /// <summary>
    /// Gets the timestamp in timezone of current <see cref="AtataContext"/>.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Gets the level of log entry.
    /// </summary>
    public LogLevel Level { get; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message { get; }

    internal static BrowserLogEntry Create(LogEntry logEntry, TimeZoneInfo destinationTimeZoneInfo) =>
        new(
            TimeZoneInfo.ConvertTimeFromUtc(logEntry.Timestamp, destinationTimeZoneInfo),
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
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(logLevel, nameof(logLevel))
        };

    private static string CorrectLineBreaksInMessage(string message) =>
        message.Replace("\\n", "\n");
}
