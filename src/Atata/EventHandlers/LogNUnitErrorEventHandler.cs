namespace Atata;

public sealed class LogNUnitErrorEventHandler : IEventHandler<AtataContextDeInitEvent>
{
    public void Handle(AtataContextDeInitEvent eventData, AtataContext context)
    {
        dynamic testResult = NUnitAdapter.GetCurrentTestResultAdapter();

        if (NUnitAdapter.IsTestResultAdapterFailed(testResult))
        {
            StringBuilder builder = new StringBuilder((string)testResult.Message)
                .AppendLine()
                .Append((string)testResult.StackTrace);

            context.Log.Error(builder.ToString());
        }
    }
}
