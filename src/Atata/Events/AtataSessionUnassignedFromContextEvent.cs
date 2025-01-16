namespace Atata;

public sealed class AtataSessionUnassignedFromContextEvent : AtataSessionEvent
{
    internal AtataSessionUnassignedFromContextEvent(AtataSession session)
        : base(session)
    {
    }
}
