namespace Atata;

/// <summary>
/// Represents the verification strategy for waitings.
/// Its <see cref="ReportFailure(IAtataExecutionUnit, string, Exception)"/> method throws <see cref="TimeoutException"/>.
/// </summary>
public sealed class WaitingVerificationStrategy : IVerificationStrategy
{
    public static WaitingVerificationStrategy Instance { get; } = new();

    public string VerificationKind => "Wait";

    public TimeSpan GetDefaultTimeout(IAtataExecutionUnit? executionUnit) =>
        (executionUnit?.Context ?? AtataContext.Current)?.WaitingTimeout ?? AtataContext.DefaultRetryTimeout;

    public TimeSpan GetDefaultRetryInterval(IAtataExecutionUnit? executionUnit) =>
        (executionUnit?.Context ?? AtataContext.Current)?.WaitingRetryInterval ?? AtataContext.DefaultRetryInterval;

    public void ReportFailure(IAtataExecutionUnit? executionUnit, string message, Exception? exception)
    {
        string completeMessage = $"Timed out waiting for {message}";
        throw new TimeoutException(completeMessage, exception);
    }
}
