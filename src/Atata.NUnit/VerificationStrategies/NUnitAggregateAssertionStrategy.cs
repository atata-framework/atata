namespace Atata.NUnit;

/// <summary>
/// Represents aggregate assertion strategy for NUnit.
/// Uses NUnit's <see cref="Assert.EnterMultipleScope"/> method for aggregate assertion.
/// </summary>
public sealed class NUnitAggregateAssertionStrategy : IAggregateAssertionStrategy
{
    public static NUnitAggregateAssertionStrategy Instance { get; } = new();

    public void Assert(IAtataExecutionUnit? executionUnit, Action action) =>
        NUnitAdapter.AssertMultiple(action);

    public void ReportFailure(IAtataExecutionUnit? executionUnit, string message, string stackTrace)
    {
        NUnitAdapter.RecordAssertionIntoTestResult(
            NUnitAssertionStatus.Failed,
            message + Environment.NewLine,
            stackTrace);
        NUnitAdapter.RecordTestCompletionIntoTestResult();
    }
}
