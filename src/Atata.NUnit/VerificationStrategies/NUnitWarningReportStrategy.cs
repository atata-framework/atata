namespace Atata.NUnit;

/// <summary>
/// Represents the NUnit strategy for warning assertion reporting.
/// Invokes <c>TestExecutionContext.CurrentContext.CurrentResult.RecordAssertion(AssertionStatus.Warning, message, stackTrace)</c>.
/// </summary>
public class NUnitWarningReportStrategy : IWarningReportStrategy
{
    public static NUnitWarningReportStrategy Instance { get; } = new();

    public void Report(IAtataExecutionUnit? executionUnit, string message, string stackTrace) =>
        NUnitAdapter.RecordAssertionIntoTestResult(
            NUnitAssertionStatus.Warning,
            message + Environment.NewLine,
            stackTrace);
}
