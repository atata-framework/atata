namespace Atata.NUnit;

[Obsolete("Don't use this class, as it will be removed. " +
    "It is recommended to inherit AtataGlobalFixture and AtataTestSuite classes for global and test suites. " +
    "Otherwise use AtataContext.HandleTestResultException(...) method on test tear down.")] // Obsolete since v4.0.0.
public sealed class LogNUnitErrorEventHandler : IEventHandler<AtataContextDeInitStartedEvent>
{
    public void Handle(AtataContextDeInitStartedEvent eventData, AtataContext context)
    {
        var testResult = TestContext.CurrentContext.Result;

        if (testResult.Outcome.IsCurrentTestFailed())
            context.HandleTestResultException(testResult.Message, testResult.StackTrace);
    }
}
