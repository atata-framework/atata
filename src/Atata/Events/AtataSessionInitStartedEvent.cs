namespace Atata;

public sealed class AtataSessionInitStartedEvent : AtataSessionEvent
{
    internal AtataSessionInitStartedEvent(AtataSession session)
        : base(session)
    {
    }
}
