namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is initialized.
/// </summary>
public sealed class AtataSessionInitCompletedEvent : AtataSessionEvent
{
    internal AtataSessionInitCompletedEvent(AtataSession session)
        : base(session)
    {
    }
}
