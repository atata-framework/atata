namespace Atata;

/// <summary>
/// Represents the logging event information raised by Atata framework.
/// </summary>
public class LogEventInfo
{
    internal LogEventInfo()
    {
    }

    /// <summary>
    /// Gets the context of the logging event.
    /// Can be <see langword="null"/>.
    /// </summary>
    public AtataContext Context { get; internal set; }

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

    [Obsolete("Use Context." + nameof(AtataContext.BuildStartInTimeZone) + " instead.")] // Obsolete since v2.0.0.
    public DateTime BuildStart => Context?.BuildStartInTimeZone ?? DateTime.MinValue;

    [Obsolete("Use Context." + nameof(AtataContext.TestName) + " instead.")] // Obsolete since v2.0.0.
    public string TestName => Context?.TestName;

    [Obsolete("Use Context." + nameof(AtataContext.TestNameSanitized) + " instead.")] // Obsolete since v2.0.0.
    public string TestNameSanitized => Context?.TestNameSanitized;

    [Obsolete("Use Context." + nameof(AtataContext.TestSuiteName) + " instead.")] // Obsolete since v2.0.0.
    public string TestSuiteName => Context?.TestSuiteName;

    [Obsolete("Use Context." + nameof(AtataContext.TestSuiteNameSanitized) + " instead.")] // Obsolete since v2.0.0.
    public string TestSuiteNameSanitized => Context?.TestSuiteNameSanitized;

    [Obsolete("Use Context." + nameof(AtataContext.StartedAt) + " instead.")] // Obsolete since v2.0.0.
    public DateTime TestStart => Context?.StartedAt ?? DateTime.MinValue;

    [Obsolete("Use Context." + nameof(AtataContext.DriverAlias) + " instead.")] // Obsolete since v2.0.0.
    public string DriverAlias => Context?.DriverAlias;
}
