namespace Atata;

public sealed class AtataSessionAssignedToContextEvent : AtataSessionEvent
{
    internal AtataSessionAssignedToContextEvent(AtataSession session)
        : base(session)
    {
    }
}
