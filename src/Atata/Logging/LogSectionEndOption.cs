namespace Atata;

/// <summary>
/// Specifies an output option of log section end.
/// </summary>
public enum LogSectionEndOption
{
    /// <summary>
    /// Include log section end.
    /// </summary>
    Include,

    /// <summary>
    /// Include log section end only for block sections: <see cref="StepLogSection"/>,
    /// <see cref="SetupLogSection"/> and <see cref="AggregateAssertionLogSection"/>.
    /// </summary>
    IncludeForBlocks,

    /// <summary>
    /// Exclude log section end.
    /// </summary>
    Exclude
}
