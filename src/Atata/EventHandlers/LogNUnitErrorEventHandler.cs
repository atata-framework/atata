namespace Atata;

public sealed class LogNUnitErrorEventHandler : IEventHandler<AtataContextDeInitEvent>
{
    public void Handle(AtataContextDeInitEvent eventData, AtataContext context)
    {
        dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

        if (NUnitAdapter.IsTestResultAdapterFailed(testResult))
        {
            string testResultMessage = testResult.Message;

            if (context.LastLoggedException is null || !testResultMessage.Contains(context.LastLoggedException.Message))
            {
                string completeErrorMessage = VerificationUtils.AppendStackTraceToFailureMessage(
                    testResultMessage,
                    (string)testResult.StackTrace);

                context.Log.Error(completeErrorMessage);
            }
        }
    }
}
