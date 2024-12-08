namespace Atata;

/// <summary>
/// Specifies a way how to start a session.
/// </summary>
public enum AtataSessionStartMode
{
    /// <summary>
    /// Build a new session.
    /// </summary>
    Build,

    /// <summary>
    /// Borrows a session from one of the ascendant contexts.
    /// </summary>
    Borrow
}
