namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is deinitialized.
/// </summary>
public sealed class AtataSessionDeInitCompletedEvent : AtataSessionEvent
{
    internal AtataSessionDeInitCompletedEvent(AtataSession session)
        : base(session)
    {
    }
}
