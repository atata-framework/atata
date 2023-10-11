namespace Atata;

public sealed class LogNUnitErrorEventHandler : IEventHandler<AtataContextDeInitEvent>
{
    public void Handle(AtataContextDeInitEvent eventData, AtataContext context)
    {
        dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

        if (NUnitAdapter.IsTestResultAdapterFailed(testResult))
            context.Log.Error((string)testResult.Message, (string)testResult.StackTrace);
    }
}
