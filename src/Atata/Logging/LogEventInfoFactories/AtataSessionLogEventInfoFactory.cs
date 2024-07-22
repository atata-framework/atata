namespace Atata;

internal sealed class AtataSessionLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly ILogEventInfoFactory _parentFactory;

    private readonly AtataSession _session;

    internal AtataSessionLogEventInfoFactory(
        ILogEventInfoFactory parentFactory,
        AtataSession session)
    {
        _parentFactory = parentFactory.CheckNotNull(nameof(parentFactory));
        _session = session.CheckNotNull(nameof(session));
    }

    public LogEventInfo Create(LogLevel level, string message)
    {
        var eventInfo = _parentFactory.Create(level, message);

        eventInfo.Session = _session;

        return eventInfo;
    }
}
