namespace Atata;

/// <summary>
/// Specifies a test result status.
/// </summary>
public enum TestResultStatus
{
    /// <summary>
    /// The undefined status.
    /// </summary>
    None,

    /// <summary>
    /// The test is inconclusive.
    /// </summary>
    Inconclusive,

    /// <summary>
    /// The test passed.
    /// </summary>
    Passed,

    /// <summary>
    /// The test had warning(s).
    /// </summary>
    Warning,

    /// <summary>
    /// The test failed.
    /// </summary>
    Failed
}
