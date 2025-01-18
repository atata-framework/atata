#nullable enable

namespace Atata;

/// <summary>
/// Represents the base class for log consumer that needs to be initialized in a lazy way.
/// </summary>
public abstract class LazyInitializableLogConsumer : ILogConsumer
{
    private readonly object _loggerInitializationLock = new();

    private volatile bool _isInitialized;

    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected dynamic? Logger { get; private set; }

    public void Log(LogEventInfo eventInfo)
    {
        EnsureLoggerIsInitialized();

        if (Logger is not null)
            OnLog(eventInfo);
    }

    private void EnsureLoggerIsInitialized()
    {
        if (!_isInitialized)
        {
            lock (_loggerInitializationLock)
            {
                if (!_isInitialized)
                {
                    try
                    {
                        Logger = GetLogger();
                    }
                    finally
                    {
                        _isInitialized = true;
                    }
                }
            }
        }
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
