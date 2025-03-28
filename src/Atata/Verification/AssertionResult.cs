#nullable enable

namespace Atata;

/// <summary>
/// Represents the result of assertion.
/// </summary>
public sealed class AssertionResult
{
    private AssertionResult(AssertionStatus status, string message, string? stackTrace = null)
    {
        Status = status;
        Message = message;
        StackTrace = stackTrace;
    }

    /// <summary>
    /// Gets the status of assertion.
    /// </summary>
    public AssertionStatus Status { get; }

    /// <summary>
    /// Gets the failure message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the stack trace of assertion.
    /// </summary>
    public string? StackTrace { get; }

    public static AssertionResult ForFailure(string message, string stackTrace) =>
        new(AssertionStatus.Failed, message, stackTrace);

    public static AssertionResult ForWarning(string message, string stackTrace) =>
        new(AssertionStatus.Warning, message, stackTrace);

    public static AssertionResult ForException(Exception exception) =>
        new(AssertionStatus.Exception, exception.ToString());
}
