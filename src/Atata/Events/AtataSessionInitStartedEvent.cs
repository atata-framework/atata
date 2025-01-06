namespace Atata;

public sealed class AtataSessionInitStartedEvent
{
    public AtataSessionInitStartedEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
