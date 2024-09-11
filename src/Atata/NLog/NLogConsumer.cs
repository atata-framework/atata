namespace Atata;

/// <summary>
/// Represents the log consumer that writes log to NLog using default NLog configuration.
/// </summary>
public class NLogConsumer : LazyInitializableLogConsumer, INamedLogConsumer
{
    public NLogConsumer()
        : this(null)
    {
    }

    public NLogConsumer(string loggerName) =>
        LoggerName = loggerName;

    /// <inheritdoc/>
    public string LoggerName { get; set; }

    /// <inheritdoc/>
    protected override dynamic GetLogger()
    {
        var logger = LoggerName != null
            ? NLogAdapter.GetLogger(LoggerName)
            : NLogAdapter.GetCurrentClassLogger();

        return logger ?? throw new InvalidOperationException("Failed to create NLog logger.");
    }

    /// <inheritdoc/>
    protected override void OnLog(LogEventInfo eventInfo)
    {
        dynamic otherEventInfo = NLogAdapter.CreateLogEventInfo(eventInfo);
        Logger.Log(otherEventInfo);
    }
}
