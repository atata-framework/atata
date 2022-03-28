namespace Atata
{
    public class LogNUnitErrorOnCleanUpEventHandler : IEventHandler<AtataContextCleanUpEvent>
    {
        public void Handle(AtataContextCleanUpEvent eventData, AtataContext context)
        {
            dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

            if (NUnitAdapter.IsTestResultAdapterFailed(testResult))
                context.Log.Error((string)testResult.Message, (string)testResult.StackTrace);
        }
    }
}
