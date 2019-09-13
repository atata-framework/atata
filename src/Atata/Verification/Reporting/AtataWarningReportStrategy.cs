namespace Atata
{
    /// <summary>
    /// Represents native/default Atata strategy for warning assertion reporting.
    /// </summary>
    public class AtataWarningReportStrategy : IWarningReportStrategy
    {
        public void Report(string message, string stackTrace)
        {
            AtataContext.Current.PendingFailureAssertionResults.Add(AssertionResult.ForWarning(message, stackTrace));
        }
    }
}
