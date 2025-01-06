namespace Atata;

public sealed class AtataSessionInitCompletedEvent
{
    public AtataSessionInitCompletedEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
