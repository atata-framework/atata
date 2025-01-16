namespace Atata;

public sealed class TakePageSnapshotOnNUnitErrorEventHandler : IConditionalEventHandler<AtataSessionDeInitStartedEvent>
{
    private readonly string _snapshotTitle;

    public TakePageSnapshotOnNUnitErrorEventHandler(string snapshotTitle = "Failed") =>
        _snapshotTitle = snapshotTitle;

    public bool CanHandle(AtataSessionDeInitStartedEvent eventData, AtataContext context) =>
        NUnitAdapter.IsCurrentTestFailed() && eventData.Session.IsActive;

    public void Handle(AtataSessionDeInitStartedEvent eventData, AtataContext context) =>
        (eventData.Session as WebSession)?.TakePageSnapshot(_snapshotTitle);
}
