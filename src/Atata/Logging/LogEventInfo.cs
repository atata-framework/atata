namespace Atata;

/// <summary>
/// Represents the logging event information raised by Atata framework.
/// </summary>
public sealed class LogEventInfo
{
    internal LogEventInfo()
    {
    }

    /// <summary>
    /// Gets the context of the logging event.
    /// </summary>
    public AtataContext Context { get; internal set; }

    /// <summary>
    /// Gets the session of the logging event.
    /// Can be <see langword="null"/> when the log event is written directly to <see cref="AtataContext"/>.
    /// </summary>
    public AtataSession Session { get; internal set; }

    /// <summary>
    /// Gets the category name.
    /// Can be <see langword="null"/> by default.
    /// </summary>
    public string Category { get; internal set; }

    /// <summary>
    /// Gets the external source.
    /// Can be <see langword="null"/> by default.
    /// </summary>
    public string ExternalSource { get; internal set; }

    /// <summary>
    /// Gets the timestamp of the logging event.
    /// </summary>
    public DateTime Timestamp { get; internal set; }

    /// <summary>
    /// Gets the level of the logging event.
    /// </summary>
    public LogLevel Level { get; internal set; }

    /// <summary>
    /// Gets the log message.
    /// </summary>
    public string Message { get; internal set; }

    /// <summary>
    /// Gets the exception information.
    /// </summary>
    public Exception Exception { get; internal set; }

    /// <summary>
    /// Gets the section start information.
    /// </summary>
    public LogSection SectionStart { get; internal set; }

    /// <summary>
    /// Gets the section end information.
    /// </summary>
    public LogSection SectionEnd { get; internal set; }

    /// <summary>
    /// Gets the nesting level.
    /// </summary>
    public int NestingLevel { get; internal set; }
}
