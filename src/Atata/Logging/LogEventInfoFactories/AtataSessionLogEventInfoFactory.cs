#nullable enable

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

    public LogEventInfo Create(DateTime timestamp, LogLevel level, string? message)
    {
        var eventInfo = _parentFactory.Create(timestamp, level, message);

        eventInfo.Session = _session;

        return eventInfo;
    }
}
