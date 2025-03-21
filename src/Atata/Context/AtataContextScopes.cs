namespace Atata;

/// <summary>
/// Specifies a set of <see cref="AtataContext"/> scopes.
/// </summary>
[Flags]
public enum AtataContextScopes
{
    /// <summary>
    /// No scopes.
    /// </summary>
    None = 0,

    /// <summary>
    /// The test scope.
    /// </summary>
    Test = 0x0000_0001,

    /// <summary>
    /// The test suite (class) scope.
    /// </summary>
    TestSuite = 0x0000_0010,

    /// <summary>
    /// The test suite group (collection fixture) scope.
    /// </summary>
    TestSuiteGroup = 0x0000_0100,

    /// <summary>
    /// The namespace-wide scope.
    /// </summary>
    Namespace = 0x0000_1000,

    /// <summary>
    /// The global (assembly-wide) scope.
    /// </summary>
    Global = 0x0001_0000,

    /// <summary>
    /// All scopes.
    /// </summary>
    All = 0x0001_1111,
}
