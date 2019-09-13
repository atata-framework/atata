namespace Atata
{
    /// <summary>
    /// Represents NUnit strategy for warning assertion reporting.
    /// </summary>
    public class NUnitWarningReportStrategy : IWarningReportStrategy
    {
        public void Report(string message, string stackTrace)
        {
            NUnitAdapter.RecordAssertionIntoTestResult(NUnitAdapter.AssertionStatus.Warning, message, stackTrace);
        }
    }
}
