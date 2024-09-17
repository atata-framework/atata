namespace Atata;

public sealed class TakePageSnapshotOnNUnitErrorEventHandler : IConditionalEventHandler<AtataSessionDeInitEvent>
{
    private readonly string _snapshotTitle;

    public TakePageSnapshotOnNUnitErrorEventHandler(string snapshotTitle = "Failed") =>
        _snapshotTitle = snapshotTitle;

    public bool CanHandle(AtataSessionDeInitEvent eventData, AtataContext context) =>
        NUnitAdapter.IsCurrentTestFailed() && eventData.Session.IsActive;

    public void Handle(AtataSessionDeInitEvent eventData, AtataContext context) =>
        (eventData.Session as WebSession)?.TakePageSnapshot(_snapshotTitle);
}
