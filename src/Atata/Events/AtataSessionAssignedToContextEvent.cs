#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is assigned to <see cref="AtataContext"/>.
/// </summary>
public sealed class AtataSessionAssignedToContextEvent : AtataSessionEvent
{
    internal AtataSessionAssignedToContextEvent(AtataSession session)
        : base(session)
    {
    }
}
