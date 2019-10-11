using System;

namespace Atata
{
    /// <summary>
    /// Represents a core part of expectation verification functionality.
    /// Its <see cref="ReportFailure(string, Exception)"/> method builds warning details, appends a warning into log,
    /// adds assertion result to <see cref="AtataContext.AssertionResults"/> collection of <see cref="AtataContext.Current"/>
    /// and finally reports a warning details to <see cref="AtataContext.WarningReportStrategy"/> of <see cref="AtataContext.Current"/>.
    /// </summary>
    public class ExpectationReportStrategy : IVerificationReportStrategy
    {
        public string VerificationKind => "Expect";

        public void ReportFailure(string message, Exception exception)
        {
            string completeMessage = $"Unexpected {message}";
            completeMessage = VerificationUtils.AppendExceptionToFailureMessage(completeMessage, exception);

            string stackTrace = VerificationUtils.BuildStackTraceForAggregateAssertion();
            AtataContext context = AtataContext.Current;

            context.AssertionResults.Add(AssertionResult.ForWarning(completeMessage, stackTrace));
            context.Log.Warn(completeMessage);

            context.WarningReportStrategy.Report(completeMessage, stackTrace);
        }
    }
}
