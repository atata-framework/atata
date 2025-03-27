#nullable enable

namespace Atata;

/// <summary>
/// Represents a core part of expectation verification functionality.
/// Its <see cref="ReportFailure(IAtataExecutionUnit, string, Exception)"/> method builds warning details, appends a warning into log,
/// adds assertion result to <see cref="AtataContext.AssertionResults"/> collection of executing <see cref="AtataContext"/>,
/// and finally reports a warning details to <see cref="AtataContext.WarningReportStrategy"/> of executing <see cref="AtataContext"/>.
/// </summary>
public sealed class ExpectationVerificationStrategy : IVerificationStrategy
{
    public static ExpectationVerificationStrategy Instance { get; } = new();

    public string VerificationKind => "Expect";

    public TimeSpan GetDefaultTimeout(IAtataExecutionUnit? executionUnit) =>
        (executionUnit?.Context ?? AtataContext.Current)?.VerificationTimeout ?? AtataContext.DefaultRetryTimeout;

    public TimeSpan GetDefaultRetryInterval(IAtataExecutionUnit? executionUnit) =>
        (executionUnit?.Context ?? AtataContext.Current)?.VerificationRetryInterval ?? AtataContext.DefaultRetryInterval;

    public void ReportFailure(IAtataExecutionUnit? executionUnit, string message, Exception? exception)
    {
        string completeMessage = $"Unexpected {message}";
        executionUnit ??= AtataContext.Current?.ExecutionUnit;

        if (executionUnit is not null)
        {
            string completeMessageWithException = VerificationUtils.AppendExceptionToFailureMessage(completeMessage, exception);

            string stackTrace = VerificationUtils.CreateStackTraceForAssertionFailiure();

            executionUnit.Context.AssertionResults.Add(AssertionResult.ForWarning(completeMessageWithException, stackTrace));
            executionUnit.Log.Warn(completeMessageWithException);

            if (executionUnit.Context.TestResultStatus == TestResultStatus.Passed)
                executionUnit.Context.TestResultStatus = TestResultStatus.Warning;

            executionUnit.Context.WarningReportStrategy.Report(executionUnit, completeMessageWithException, stackTrace);
        }
        else
        {
            throw new InvalidOperationException(
                $"Cannot report warning to {nameof(AtataContext)}.{nameof(AtataContext.Current)} as current context is null.",
                VerificationUtils.CreateAssertionException(null, completeMessage, exception));
        }
    }
}
