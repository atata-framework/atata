namespace Atata;

public sealed class LogNUnitErrorEventHandler : IEventHandler<AtataContextDeInitStartedEvent>
{
    public void Handle(AtataContextDeInitStartedEvent eventData, AtataContext context)
    {
        dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

        if (NUnitAdapter.IsTestResultAdapterFailed(testResult))
        {
            string testResultMessage = testResult.Message;
            string testResultStackTrace = testResult.StackTrace;

            context.HandleTestResultException(testResultMessage, testResultStackTrace);
        }
    }
}
