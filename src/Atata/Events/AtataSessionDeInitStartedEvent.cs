namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is started to deinitialize.
/// </summary>
public sealed class AtataSessionDeInitStartedEvent
{
    internal AtataSessionDeInitStartedEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
