namespace Atata;

/// <summary>
/// Specifies the condition under which logging should be skipped depending on a test result status.
/// </summary>
public enum SkipLogCondition
{
    /// <summary>
    /// No skipping of log.
    /// </summary>
    None,

    /// <summary>
    /// Skips log when a test result status is <see cref="TestResultStatus.Passed"/>.
    /// </summary>
    Passed,

    /// <summary>
    /// Skips log when a test result status is either <see cref="TestResultStatus.Passed"/> or <see cref="TestResultStatus.Inconclusive"/>.
    /// </summary>
    PassedOrInconclusive,

    /// <summary>
    /// Skips log when a test result status is either <see cref="TestResultStatus.Passed"/>, <see cref="TestResultStatus.Inconclusive"/> or <see cref="TestResultStatus.Warning"/>.
    /// </summary>
    PassedOrInconclusiveOrWarning
}
