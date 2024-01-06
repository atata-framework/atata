namespace Atata;

/// <summary>
/// Represents the NUnit strategy for warning assertion reporting.
/// Invokes <c>TestExecutionContext.CurrentContext.CurrentResult.RecordAssertion(AssertionStatus.Warning, message, stackTrace)</c>.
/// </summary>
public class NUnitWarningReportStrategy : IWarningReportStrategy
{
    public void Report(string message, string stackTrace) =>
        NUnitAdapter.RecordAssertionIntoTestResult(
            NUnitAdapter.AssertionStatus.Warning,
            message + Environment.NewLine,
            stackTrace);
}
