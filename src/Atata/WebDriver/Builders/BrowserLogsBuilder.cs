namespace Atata;

/// <summary>
/// Represents the builder of browser logs monitoring and handling.
/// </summary>
public sealed class BrowserLogsBuilder : ICloneable
{
    /// <summary>
    /// Gets or sets a value indicating whether the browser log should be transferred
    /// to Atata logging system as "Browser" external source.
    /// The default value is <see langword="false"/>.
    /// </summary>
    public bool Log { get; set; }

    /// <summary>
    /// Gets or sets the minimum log level on which to raise warnings.
    /// The default value is <see langword="null"/>, meaning that warning is disabled.
    /// For example, setting the <see cref="LogLevel.Warn"/> value will mean to warn on
    /// browser log entries with <see cref="LogLevel.Warn"/> level and higher,
    /// which are <see cref="LogLevel.Error"/> and <see cref="LogLevel.Fatal"/>.
    /// </summary>
    public LogLevel? MinLevelOfWarning { get; set; }

    internal bool HasPropertiesToUse =>
        Log || MinLevelOfWarning is not null;

    /// <summary>
    /// Sets a value indicating whether the browser log should be transferred
    /// to Atata logging system as "Browser" external source.
    /// The default value is <see langword="false"/>.
    /// </summary>
    /// <param name="enable">Whether to enable logging.</param>
    /// <returns>The same <see cref="BrowserLogsBuilder"/> instance.</returns>
    public BrowserLogsBuilder UseLog(bool enable = true)
    {
        Log = enable;
        return this;
    }

    /// <summary>
    /// Sets the minimum log level on which to raise warnings.
    /// The default value is <see langword="null"/>, meaning that warning is disabled.
    /// For example, setting the <see cref="LogLevel.Warn"/> value will mean to warn on
    /// browser log entries with <see cref="LogLevel.Warn"/> level and higher,
    /// which are <see cref="LogLevel.Error"/> and <see cref="LogLevel.Fatal"/>.
    /// </summary>
    /// <param name="minLevel">The minimum log level.</param>
    /// <returns>The same <see cref="BrowserLogsBuilder"/> instance.</returns>
    public BrowserLogsBuilder UseMinLevelOfWarning(LogLevel? minLevel)
    {
        MinLevelOfWarning = minLevel;
        return this;
    }

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    internal BrowserLogsBuilder Clone() =>
        (BrowserLogsBuilder)MemberwiseClone();
}
