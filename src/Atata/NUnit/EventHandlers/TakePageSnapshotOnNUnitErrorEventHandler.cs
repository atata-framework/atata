namespace Atata;

public sealed class TakePageSnapshotOnNUnitErrorEventHandler : IConditionalEventHandler<AtataContextDeInitEvent>
{
    private readonly string _snapshotTitle;

    public TakePageSnapshotOnNUnitErrorEventHandler(string snapshotTitle = "Failed") =>
        _snapshotTitle = snapshotTitle;

    public bool CanHandle(AtataContextDeInitEvent eventData, AtataContext context) =>
        NUnitAdapter.IsCurrentTestFailed() && context.HasDriver;

    public void Handle(AtataContextDeInitEvent eventData, AtataContext context) =>
        context.TakePageSnapshot(_snapshotTitle);
}
