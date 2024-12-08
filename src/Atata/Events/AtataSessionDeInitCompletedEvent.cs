namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is deinitialized.
/// </summary>
public sealed class AtataSessionDeInitCompletedEvent
{
    public AtataSessionDeInitCompletedEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
