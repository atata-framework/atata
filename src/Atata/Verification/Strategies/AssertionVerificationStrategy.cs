namespace Atata;

public class AssertionVerificationStrategy : IVerificationStrategy
{
    public string VerificationKind => "Assert";

    public TimeSpan DefaultTimeout =>
        AtataContext.Current?.VerificationTimeout ?? AtataContext.DefaultRetryTimeout;

    public TimeSpan DefaultRetryInterval =>
        AtataContext.Current?.VerificationRetryInterval ?? AtataContext.DefaultRetryInterval;

    public void ReportFailure(string message, Exception exception)
    {
        string completeMessage = $"Wrong {message}";

        string completeMessageWithException = VerificationUtils.AppendExceptionToFailureMessage(completeMessage, exception);
        string stackTrace = VerificationUtils.BuildStackTraceForAggregateAssertion();

        AtataContext context = AtataContext.Current;

        if (context != null)
        {
            context.AssertionResults.Add(AssertionResult.ForFailure(completeMessageWithException, stackTrace));

            if (context.AggregateAssertionLevel > 0)
            {
                context.Log.Error(completeMessage);
                context.AggregateAssertionStrategy.ReportFailure(completeMessageWithException, stackTrace);
                return;
            }
        }

        throw VerificationUtils.CreateAssertionException(completeMessage, exception);
    }
}
