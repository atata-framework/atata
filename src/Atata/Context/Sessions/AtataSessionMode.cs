namespace Atata;

/// <summary>
/// Specifies a way how to use a session.
/// </summary>
public enum AtataSessionMode
{
    /// <summary>
    /// The own session mode.
    /// Such session belongs only to the context that created it.
    /// </summary>
    Own,

    /// <summary>
    /// The shared session mode.
    /// Such session can be borrowed by child contexts.
    /// </summary>
    Shared,

    /// <summary>
    /// The pool session mode.
    /// Sessions from a pool can be taken by the owner context as well as descendant contexts.
    /// When a descendant context ends, the taken session returns back to the pool.
    /// </summary>
    Pool
}
