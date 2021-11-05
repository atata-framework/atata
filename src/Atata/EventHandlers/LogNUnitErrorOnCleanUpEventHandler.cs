namespace Atata
{
    public class LogNUnitErrorOnCleanUpEventHandler : IConditionalEventHandler<AtataContextCleanUpEvent>
    {
        public bool CanHandle(AtataContextCleanUpEvent eventData, AtataContext context)
        {
            dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

            return NUnitAdapter.IsTestResultAdapterFailed(testResult);
        }

        public void Handle(AtataContextCleanUpEvent eventData, AtataContext context)
        {
            dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

            context.Log.Error((string)testResult.Message, (string)testResult.StackTrace);
        }
    }
}
