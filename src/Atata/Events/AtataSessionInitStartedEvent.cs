#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is started to initialize.
/// </summary>
public sealed class AtataSessionInitStartedEvent : AtataSessionEvent
{
    internal AtataSessionInitStartedEvent(AtataSession session)
        : base(session)
    {
    }
}
