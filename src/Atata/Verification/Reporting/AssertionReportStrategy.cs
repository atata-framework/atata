using System;

namespace Atata
{
    public class AssertionReportStrategy : IVerificationReportStrategy
    {
        public string VerificationKind => "Assert";

        public void ReportFailure(string message, Exception exception)
        {
            string completeMessage = $"Wrong {message}";

            string completeMessageWithException = VerificationUtils.AppendExceptionToFailureMessage(completeMessage, exception);
            string stackTrace = VerificationUtils.BuildStackTraceForAggregateAssertion();

            AtataContext context = AtataContext.Current;
            context.AssertionResults.Add(AssertionResult.ForFailure(completeMessageWithException, stackTrace));

            if (context.AggregateAssertionLevel == 0)
            {
                throw VerificationUtils.CreateAssertionException(completeMessage, exception);
            }
            else
            {
                context.Log.Error(completeMessage);
                context.AggregateAssertionStrategy.ReportFailure(completeMessageWithException, stackTrace);
            }
        }
    }
}
