namespace Atata;

public sealed class AtataSessionInitStartedEvent
{
    internal AtataSessionInitStartedEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
