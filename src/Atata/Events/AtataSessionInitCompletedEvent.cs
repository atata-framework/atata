namespace Atata;

public sealed class AtataSessionInitCompletedEvent
{
    internal AtataSessionInitCompletedEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
