namespace Atata;

/// <summary>
/// Represents a configuration builder of browser logs monitoring and handling.
/// </summary>
public sealed class BrowserLogsWebDriverSessionBuilder
{
    private readonly WebDriverSessionBuilder _sessionBuilder;

    internal BrowserLogsWebDriverSessionBuilder(WebDriverSessionBuilder sessionBuilder) =>
        _sessionBuilder = sessionBuilder;

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
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseLog(bool enable = true)
    {
        Log = enable;
        return _sessionBuilder;
    }

    /// <summary>
    /// Sets the minimum log level on which to raise warnings.
    /// The default value is <see langword="null"/>, meaning that warning is disabled.
    /// For example, setting the <see cref="LogLevel.Warn"/> value will mean to warn on
    /// browser log entries with <see cref="LogLevel.Warn"/> level and higher,
    /// which are <see cref="LogLevel.Error"/> and <see cref="LogLevel.Fatal"/>.
    /// </summary>
    /// <param name="minLevel">The minimum log level.</param>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseMinLevelOfWarning(LogLevel? minLevel)
    {
        MinLevelOfWarning = minLevel;
        return _sessionBuilder;
    }

    internal BrowserLogsWebDriverSessionBuilder CloneFor(WebDriverSessionBuilder sessionBuilder) =>
        new(sessionBuilder)
        {
            Log = Log,
            MinLevelOfWarning = MinLevelOfWarning
        };
}
