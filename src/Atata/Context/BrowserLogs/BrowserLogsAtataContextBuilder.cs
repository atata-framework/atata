namespace Atata;

/// <summary>
/// Represents the builder of a browser logs configuration.
/// </summary>
public sealed class BrowserLogsAtataContextBuilder : AtataContextBuilder
{
    internal BrowserLogsAtataContextBuilder(AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
    }

    /// <summary>
    /// Sets a value indicating whether the browser log should be transferred to Atata logging system.
    /// The default value is <see langword="false"/>.
    /// </summary>
    /// <param name="enable">Whether to enable logging.</param>
    /// <returns>The <see cref="BrowserLogsAtataContextBuilder"/> instance.</returns>
    public BrowserLogsAtataContextBuilder UseLog(bool enable = true)
    {
        BuildingContext.BrowserLogs.Log = enable;
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
    /// <returns>The <see cref="BrowserLogsAtataContextBuilder"/> instance.</returns>
    public BrowserLogsAtataContextBuilder UseMinLevelOfWarning(LogLevel? minLevel)
    {
        BuildingContext.BrowserLogs.MinLevelOfWarning = minLevel;
        return this;
    }
}
