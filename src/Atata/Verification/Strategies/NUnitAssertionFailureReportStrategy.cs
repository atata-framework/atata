namespace Atata;

/// <summary>
/// Represents the NUnit strategy for assertion failure reporting.
/// </summary>
public sealed class NUnitAssertionFailureReportStrategy : IAssertionFailureReportStrategy
{
    public static NUnitAssertionFailureReportStrategy Instance { get; } = new();

    /// <inheritdoc/>
    public void Report(string message, Exception exception, string stackTrace)
    {
        NUnitAdapter.RecordAssertionIntoTestResult(
            NUnitAdapter.AssertionStatus.Failed,
            VerificationUtils.AppendExceptionToFailureMessage(message, exception) + Environment.NewLine,
            stackTrace);
        NUnitAdapter.RecordTestCompletionIntoTestResult();

        string completeErrorMessage = NUnitAdapter.GetTestResultMessage();

        var assertionException = VerificationUtils.CreateAssertionException(completeErrorMessage);

        if (AtataContext.Current is AtataContext context)
            context.LastLoggedException = assertionException;

        throw assertionException;
    }
}
