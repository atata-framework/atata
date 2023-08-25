namespace Atata;

public class AssertionVerificationStrategy : IVerificationStrategy
{
    private readonly AtataContext _context;

    public AssertionVerificationStrategy()
        : this(null)
    {
    }

    public AssertionVerificationStrategy(AtataContext context) =>
        _context = context;

    public string VerificationKind => "Assert";

    public TimeSpan DefaultTimeout =>
        (_context ?? AtataContext.Current)?.VerificationTimeout ?? AtataContext.DefaultRetryTimeout;

    public TimeSpan DefaultRetryInterval =>
        (_context ?? AtataContext.Current)?.VerificationRetryInterval ?? AtataContext.DefaultRetryInterval;

    public void ReportFailure(string message, Exception exception)
    {
        string completeMessage = $"Wrong {message}";
        AtataContext context = _context ?? AtataContext.Current;

        if (context != null)
        {
            string completeMessageWithException = VerificationUtils.AppendExceptionToFailureMessage(completeMessage, exception);
            string stackTrace = VerificationUtils.BuildStackTraceForAggregateAssertion();

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
