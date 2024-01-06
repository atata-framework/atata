namespace Atata;

/// <summary>
/// Represents the native/default Atata strategy for assertion failure reporting.
/// Creates and throws an assertion exception with a message containing the current assertion failure
/// as well as previously recorded warnings.
/// </summary>
public sealed class AtataAssertionFailureReportStrategy : IAssertionFailureReportStrategy
{
    public static AtataAssertionFailureReportStrategy Instance { get; } = new();

    /// <inheritdoc/>
    public void Report(string message, Exception exception, string stackTrace)
    {
        AtataContext context = AtataContext.Current;

        var pendingFailureAssertionResults = context?.GetAndClearPendingFailureAssertionResults()
            ?? Array.Empty<AssertionResult>();

        Exception assertionException = pendingFailureAssertionResults.Count > 0
            ? VerificationUtils.CreateAggregateAssertionException(
                pendingFailureAssertionResults.Concat(
                    [AssertionResult.ForFailure(VerificationUtils.AppendExceptionToFailureMessage(message, exception), stackTrace)]))
            : VerificationUtils.CreateAssertionException(message, exception);

        if (context is not null)
            context.LastLoggedException = assertionException;

        throw assertionException;
    }
}
