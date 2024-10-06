namespace Atata;

public sealed class AssertionVerificationStrategy : IVerificationStrategy
{
    public static AssertionVerificationStrategy Instance { get; } = new();

    public string VerificationKind => "Assert";

    public TimeSpan GetDefaultTimeout(IAtataExecutionUnit executionUnit) =>
        (executionUnit?.Context ?? AtataContext.Current)?.VerificationTimeout ?? AtataContext.DefaultRetryTimeout;

    public TimeSpan GetDefaultRetryInterval(IAtataExecutionUnit executionUnit) =>
        (executionUnit?.Context ?? AtataContext.Current)?.VerificationRetryInterval ?? AtataContext.DefaultRetryInterval;

    public void ReportFailure(IAtataExecutionUnit executionUnit, string message, Exception exception)
    {
        string completeMessage = $"Wrong {message}";
        executionUnit ??= AtataContext.Current?.ExecutionUnit;

        string stackTrace = VerificationUtils.CreateStackTraceForAssertionFailiure();
        string completeMessageWithException = VerificationUtils.AppendExceptionToFailureMessage(completeMessage, exception);

        if (executionUnit is not null)
        {
            executionUnit.Context.AssertionResults.Add(AssertionResult.ForFailure(completeMessageWithException, stackTrace));

            string completeMessageWithExceptionAndStackTrace = VerificationUtils.AppendStackTraceToFailureMessage(
                completeMessageWithException,
                stackTrace);

            executionUnit.Log.Error(completeMessageWithExceptionAndStackTrace);

            if (executionUnit.Context.AggregateAssertionLevel > 0)
            {
                executionUnit.Context.AggregateAssertionStrategy.ReportFailure(executionUnit, completeMessageWithException, stackTrace);
            }
            else
            {
                executionUnit.Context.AssertionFailureReportStrategy.Report(executionUnit, completeMessage, exception, stackTrace);
            }
        }
        else
        {
            AtataAssertionFailureReportStrategy.Instance.Report(executionUnit, completeMessage, exception, stackTrace);
        }
    }
}
