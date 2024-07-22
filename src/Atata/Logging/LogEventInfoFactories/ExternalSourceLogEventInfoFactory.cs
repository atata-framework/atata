namespace Atata;

internal sealed class ExternalSourceLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly ILogEventInfoFactory _parentFactory;

    private readonly string _externalSource;

    internal ExternalSourceLogEventInfoFactory(
        ILogEventInfoFactory parentFactory,
        string externalSource)
    {
        _parentFactory = parentFactory.CheckNotNull(nameof(parentFactory));
        _externalSource = externalSource.CheckNotNullOrWhitespace(nameof(externalSource));
    }

    public LogEventInfo Create(LogLevel level, string message)
    {
        var eventInfo = _parentFactory.Create(level, message);

        eventInfo.ExternalSource = _externalSource;

        return eventInfo;
    }
}
