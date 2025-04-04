namespace Atata;

/// <summary>
/// Represents a native/default Atata strategy for warning assertion reporting.
/// Adds <see cref="AssertionResult"/> object of warning kind to
/// <see cref="AtataContext.PendingFailureAssertionResults"/> collection of executing <see cref="AtataContext"/>.
/// </summary>
public sealed class AtataWarningReportStrategy : IWarningReportStrategy
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static AtataWarningReportStrategy Instance { get; } = new();

    /// <inheritdoc/>
    public void Report(IAtataExecutionUnit? executionUnit, string message, string stackTrace)
    {
        AtataContext context = executionUnit?.Context ?? AtataContext.ResolveCurrent();

        context.PendingFailureAssertionResults.Add(
            AssertionResult.ForWarning(message, stackTrace));
    }
}
