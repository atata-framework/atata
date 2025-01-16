namespace Atata;

public sealed class AtataSessionInitCompletedEvent : AtataSessionEvent
{
    internal AtataSessionInitCompletedEvent(AtataSession session)
        : base(session)
    {
    }
}
