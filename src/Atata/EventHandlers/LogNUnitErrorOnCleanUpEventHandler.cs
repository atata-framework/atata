namespace Atata;

[Obsolete("Use " + nameof(LogNUnitErrorEventHandler) + " instead.")] // Obsolete since v2.11.0.
public class LogNUnitErrorOnCleanUpEventHandler : IEventHandler<AtataContextCleanUpEvent>
{
    public void Handle(AtataContextCleanUpEvent eventData, AtataContext context)
    {
        dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

        if (NUnitAdapter.IsTestResultAdapterFailed(testResult))
        {
            string testResultMessage = testResult.Message;

            if (context.LastLoggedException is null || !testResultMessage.Contains(context.LastLoggedException.Message))
                context.Log.Error((string)testResult.Message, (string)testResult.StackTrace);
        }
    }
}
