namespace Atata;

[Obsolete("Use " + nameof(LogNUnitErrorEventHandler) + " instead.")] // Obsolete since v2.11.0.
public class LogNUnitErrorOnCleanUpEventHandler : IEventHandler<AtataContextCleanUpEvent>
{
    public void Handle(AtataContextCleanUpEvent eventData, AtataContext context)
    {
        dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

        if (NUnitAdapter.IsTestResultAdapterFailed(testResult))
            context.Log.Error((string)testResult.Message, (string)testResult.StackTrace);
    }
}
