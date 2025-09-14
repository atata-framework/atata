namespace Atata;

/// <summary>
/// Specifies a condition depending on a test result status.
/// </summary>
public enum TestResultStatusCondition
{
    /// <summary>
    /// No condition.
    /// </summary>
    None,

    /// <summary>
    /// A test result status is <see cref="TestResultStatus.Passed"/>.
    /// </summary>
    Passed,

    /// <summary>
    /// A test result status is either <see cref="TestResultStatus.Passed"/> or <see cref="TestResultStatus.Inconclusive"/>.
    /// </summary>
    PassedOrInconclusive,

    /// <summary>
    /// A test result status is either <see cref="TestResultStatus.Passed"/>, <see cref="TestResultStatus.Inconclusive"/> or <see cref="TestResultStatus.Warning"/>.
    /// </summary>
    PassedOrInconclusiveOrWarning
}
