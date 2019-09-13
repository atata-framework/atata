using System;

namespace Atata
{
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
