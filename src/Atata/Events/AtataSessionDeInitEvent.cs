namespace Atata;

public sealed class AtataSessionDeInitEvent
{
    public AtataSessionDeInitEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
