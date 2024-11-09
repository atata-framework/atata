namespace Atata;

public sealed class LogNUnitErrorEventHandler : IEventHandler<AtataContextDeInitEvent>
{
    public void Handle(AtataContextDeInitEvent eventData, AtataContext context)
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
