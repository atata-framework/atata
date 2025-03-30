namespace Atata.NLog;

/// <summary>
/// A log consumer that writes log to NLog using default NLog configuration.
/// </summary>
public class NLogConsumer : IInitializableLogConsumer, INamedLogConsumer
{
    private Logger _logger = null!;

    public NLogConsumer()
        : this(null)
    {
    }

    public NLogConsumer(string? loggerName) =>
        LoggerName = loggerName;

    /// <inheritdoc/>
    public string? LoggerName { get; set; }

    void IInitializableLogConsumer.Initialize(AtataContext context) =>
        _logger = LoggerName is not null
            ? NLogManager.GetLogger(LoggerName)
            : NLogManager.GetCurrentClassLogger();

    void ILogConsumer.Log(LogEventInfo eventInfo)
    {
        NLogEventInfo otherEventInfo = NLogAdapter.CreateLogEventInfo(eventInfo);
        _logger.Log(otherEventInfo);
    }

    object ICloneable.Clone() =>
        new NLogConsumer(LoggerName);
}
