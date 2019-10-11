namespace Atata
{
    /// <summary>
    /// Represents the native/default Atata strategy for warning assertion reporting.
    /// Adds <see cref="AssertionResult"/> object of warning kind to <see cref="AtataContext.PendingFailureAssertionResults"/> collection of <see cref="AtataContext.Current"/>.
    /// </summary>
    public class AtataWarningReportStrategy : IWarningReportStrategy
    {
        public void Report(string message, string stackTrace)
        {
            AtataContext.Current.PendingFailureAssertionResults.Add(AssertionResult.ForWarning(message, stackTrace));
        }
    }
}
