namespace Atata;

/// <summary>
/// Represents a core part of expectation verification functionality.
/// Its <see cref="ReportFailure(string, Exception)"/> method builds warning details, appends a warning into log,
/// adds assertion result to <see cref="AtataContext.AssertionResults"/> collection of <see cref="AtataContext.Current"/>
/// and finally reports a warning details to <see cref="AtataContext.WarningReportStrategy"/> of <see cref="AtataContext.Current"/>.
/// </summary>
public class ExpectationVerificationStrategy : IVerificationStrategy
{
    private readonly AtataContext _context;

    public ExpectationVerificationStrategy()
        : this(null)
    {
    }

    public ExpectationVerificationStrategy(AtataContext context) =>
        _context = context;

    public string VerificationKind => "Expect";

    public TimeSpan DefaultTimeout =>
        (_context ?? AtataContext.Current)?.VerificationTimeout ?? AtataContext.DefaultRetryTimeout;

    public TimeSpan DefaultRetryInterval =>
        (_context ?? AtataContext.Current)?.VerificationRetryInterval ?? AtataContext.DefaultRetryInterval;

    public void ReportFailure(string message, Exception exception)
    {
        string completeMessage = $"Unexpected {message}";
        AtataContext context = _context ?? AtataContext.Current;

        if (context != null)
        {
            string completeMessageWithException = VerificationUtils.AppendExceptionToFailureMessage(completeMessage, exception);

            string stackTrace = VerificationUtils.CreateStackTraceForAssertionFailiure();

            context.AssertionResults.Add(AssertionResult.ForWarning(completeMessageWithException, stackTrace));
            context.Log.Warn(completeMessageWithException);

            context.WarningReportStrategy.Report(completeMessageWithException, stackTrace);
        }
        else
        {
            throw new InvalidOperationException(
                $"Cannot report warning to {nameof(AtataContext)}.{nameof(AtataContext.Current)} as current context is null.",
                VerificationUtils.CreateAssertionException(completeMessage, exception));
        }
    }
}
