namespace Atata;

public sealed class AtataSessionAssignedToContextEvent
{
    internal AtataSessionAssignedToContextEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
