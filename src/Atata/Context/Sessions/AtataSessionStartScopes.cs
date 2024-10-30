namespace Atata;

/// <summary>
/// Specifies <see cref="AtataContext"/> scopes for which an <see cref="AtataSession"/> should automatically start.
/// </summary>
[Flags]
public enum AtataSessionStartScopes
{
    /// <summary>
    /// Should not start automatically, but can be started on-demand.
    /// </summary>
    None = 0,

    /// <summary>
    /// Starts upon build of <see cref="AtataContext"/> with <see cref="AtataContextScope.Test"/> scope.
    /// </summary>
    Test = 0x0000_0001,

    /// <summary>
    /// Starts upon build of <see cref="AtataContext"/> with <see cref="AtataContextScope.TestSuite"/> scope.
    /// </summary>
    TestSuite = 0x0000_0010,

    /// <summary>
    /// Starts upon build of <see cref="AtataContext"/> with <see cref="AtataContextScope.TestSuiteGroup"/> scope.
    /// </summary>
    TestSuiteGroup = 0x0000_0100,

    /// <summary>
    /// Starts upon build of <see cref="AtataContext"/> with <see cref="AtataContextScope.NamespaceSuite"/> scope.
    /// </summary>
    NamespaceSuite = 0x0000_1000,

    /// <summary>
    /// Starts upon build of <see cref="AtataContext"/> with <see cref="AtataContextScope.Global"/> scope.
    /// </summary>
    Global = 0x0001_0000
}
