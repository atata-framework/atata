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

        string stackTrace = VerificationUtils.CreateStackTraceForAssertionFailiure();
        string completeMessageWithException = VerificationUtils.AppendExceptionToFailureMessage(completeMessage, exception);

        if (context != null)
        {
            context.AssertionResults.Add(AssertionResult.ForFailure(completeMessageWithException, stackTrace));

            string completeMessageWithExceptionAndStackTrace = VerificationUtils.AppendStackTraceToFailureMessage(
                completeMessageWithException,
                stackTrace);
            context.Log.Error(completeMessageWithExceptionAndStackTrace);

            if (context.AggregateAssertionLevel > 0)
            {
                context.AggregateAssertionStrategy.ReportFailure(completeMessageWithException, stackTrace);
            }
            else
            {
                context.AssertionFailureReportStrategy.Report(completeMessage, exception, stackTrace);
            }
        }
        else
        {
            AtataAssertionFailureReportStrategy.Instance.Report(completeMessage, exception, stackTrace);
        }
    }
}
