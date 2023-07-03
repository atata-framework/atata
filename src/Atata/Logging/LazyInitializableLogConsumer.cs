namespace Atata;

/// <summary>
/// Represents the base class for log consumer that needs to be initialized in a lazy way.
/// </summary>
public abstract class LazyInitializableLogConsumer : ILogConsumer
{
    private readonly Lazy<dynamic> _lazyLogger;

    protected LazyInitializableLogConsumer() =>
        _lazyLogger = new Lazy<dynamic>(GetLogger);

    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected dynamic Logger =>
        _lazyLogger.Value;

    public void Log(LogEventInfo eventInfo)
    {
        if (Logger is not null)
            OnLog(eventInfo);
    }

    /// <summary>
    /// Logs the specified event information.
    /// </summary>
    /// <param name="eventInfo">The event information.</param>
    protected abstract void OnLog(LogEventInfo eventInfo);

    /// <summary>
    /// Gets a logger to set to <see cref="Logger"/> property and later use for logging.
    /// </summary>
    /// <returns>A logger instance.</returns>
    protected abstract dynamic GetLogger();
}
