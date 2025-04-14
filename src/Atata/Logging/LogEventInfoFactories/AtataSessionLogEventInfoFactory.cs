namespace Atata;

internal sealed class AtataSessionLogEventInfoFactory : ILogEventInfoFactory
{
    private readonly ILogEventInfoFactory _parentFactory;

    private readonly AtataSession _session;

    internal AtataSessionLogEventInfoFactory(
        ILogEventInfoFactory parentFactory,
        AtataSession session)
    {
        Guard.ThrowIfNull(parentFactory);
        Guard.ThrowIfNull(session);

        _parentFactory = parentFactory;
        _session = session;
    }

    public LogEventInfo Create(DateTime timestamp, LogLevel level, string? message)
    {
        var eventInfo = _parentFactory.Create(timestamp, level, message);

        eventInfo.Session = _session;

        return eventInfo;
    }
}
