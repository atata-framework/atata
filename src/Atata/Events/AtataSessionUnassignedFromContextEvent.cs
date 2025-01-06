namespace Atata;

public sealed class AtataSessionUnassignedFromContextEvent
{
    public AtataSessionUnassignedFromContextEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
