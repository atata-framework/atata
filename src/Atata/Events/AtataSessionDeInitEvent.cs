namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataSession"/> is deinitializing.
/// </summary>
public sealed class AtataSessionDeInitEvent
{
    public AtataSessionDeInitEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
