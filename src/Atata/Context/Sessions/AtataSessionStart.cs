namespace Atata;

/// <summary>
/// Specifies a moment of <see cref="AtataSession"/> start.
/// </summary>
public enum AtataSessionStart
{
    /// <summary>
    /// Should not start.
    /// </summary>
    None,

    /// <summary>
    /// Starts upon <see cref="AtataContext"/> build.
    /// </summary>
    OnContextBuild,

    /// <summary>
    /// Starts upon <see cref="AtataContext"/> build, only when context is for test (not global or test suite).
    /// </summary>
    OnTestContextBuild,

    /// <summary>
    /// Starts on-demand, when a session is first time requested from <see cref="AtataContext"/>.
    /// </summary>
    OnDemand
}
