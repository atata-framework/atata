namespace Atata.NUnit;

public sealed class LogNUnitErrorEventHandler : IEventHandler<AtataContextDeInitStartedEvent>
{
    public void Handle(AtataContextDeInitStartedEvent eventData, AtataContext context)
    {
        var testResult = TestContext.CurrentContext.Result;

        if (testResult.Outcome.IsCurrentTestFailed())
            context.HandleTestResultException(testResult.Message, testResult.StackTrace);
    }
}
