namespace Atata.NUnit;

/// <summary>
/// Represents the NUnit strategy for assertion failure reporting.
/// </summary>
public sealed class NUnitAssertionFailureReportStrategy : IAssertionFailureReportStrategy
{
    public static NUnitAssertionFailureReportStrategy Instance { get; } = new();

    /// <inheritdoc/>
    public void Report(IAtataExecutionUnit? executionUnit, string message, Exception? exception, string stackTrace)
    {
        AtataContext? context = executionUnit?.Context ?? AtataContext.Current;

        NUnitAdapter.RecordAssertionIntoTestResult(
            NUnitAssertionStatus.Failed,
            VerificationUtils.AppendExceptionToFailureMessage(message, exception) + Environment.NewLine,
            stackTrace);
        NUnitAdapter.RecordTestCompletionIntoTestResult();

        string completeErrorMessage = NUnitAdapter.GetTestResultMessage();

        throw VerificationUtils.CreateAssertionException(context, completeErrorMessage);
    }
}
