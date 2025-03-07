namespace Atata.NUnit;

[Obsolete("Don't use this class, as it will be removed. " +
    "It is recommended to inherit AtataGlobalFixture and AtataTestSuite classes for global and test suites. " +
    "Otherwise use AtataContext.HandleTestResultException(...) method on test tear down.")] // Obsolete since v4.0.0.
public sealed class TakePageSnapshotOnNUnitErrorEventHandler : IConditionalEventHandler<AtataSessionDeInitStartedEvent>
{
    private readonly string _snapshotTitle;

    public TakePageSnapshotOnNUnitErrorEventHandler(string snapshotTitle = "Failed") =>
        _snapshotTitle = snapshotTitle;

    public bool CanHandle(AtataSessionDeInitStartedEvent eventData, AtataContext context) =>
        TestContext.CurrentContext.Result.Outcome.IsCurrentTestFailed() && eventData.Session.IsActive;

    public void Handle(AtataSessionDeInitStartedEvent eventData, AtataContext context) =>
        (eventData.Session as WebSession)?.TakePageSnapshot(_snapshotTitle);
}
