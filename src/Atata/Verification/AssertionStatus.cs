namespace Atata;

/// <summary>
/// Specifies an assertion status.
/// </summary>
public enum AssertionStatus
{
    /// <summary>
    /// Assertion condition is passed.
    /// </summary>
    Passed,

    /// <summary>
    /// Expected assertion condition is not met but the execution was not terminated.
    /// </summary>
    Warning,

    /// <summary>
    /// Assertion condition is not met.
    /// </summary>
    Failed,

    /// <summary>
    /// An exception occurred during assertion.
    /// </summary>
    Exception
}
