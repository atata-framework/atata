namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is unassigned from <see cref="AtataContext"/>.
/// </summary>
public sealed class AtataSessionUnassignedFromContextEvent : AtataSessionEvent
{
    internal AtataSessionUnassignedFromContextEvent(AtataSession session)
        : base(session)
    {
    }
}
