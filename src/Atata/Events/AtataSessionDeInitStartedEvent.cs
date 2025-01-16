namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is started to deinitialize.
/// </summary>
public sealed class AtataSessionDeInitStartedEvent : AtataSessionEvent
{
    internal AtataSessionDeInitStartedEvent(AtataSession session)
        : base(session)
    {
    }
}
