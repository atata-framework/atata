namespace Atata;

/// <summary>
/// Specifies the scope of <see cref="AtataContext"/>.
/// </summary>
public enum AtataContextScope
{
    /// <summary>
    /// The test scope.
    /// </summary>
    Test,

    /// <summary>
    /// The test suite (class) scope.
    /// </summary>
    TestSuite,

    /// <summary>
    /// The test suite group (collection fixture) scope.
    /// </summary>
    TestSuiteGroup,

    /// <summary>
    /// The namespace-wide suite scope.
    /// </summary>
    NamespaceSuite,

    /// <summary>
    /// The global (assembly-wide) scope.
    /// </summary>
    Global
}
