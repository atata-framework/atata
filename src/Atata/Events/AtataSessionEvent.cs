namespace Atata;

/// <summary>
/// Represents a base class for events associated with <see cref="AtataSession"/>.
/// </summary>
public abstract class AtataSessionEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AtataSessionEvent"/> class.
    /// </summary>
    /// <param name="session">The session.</param>
    protected AtataSessionEvent(AtataSession session) =>
        Session = session;

    /// <summary>
    /// Gets the session.
    /// </summary>
    public AtataSession Session { get; }
}
