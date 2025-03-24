namespace Atata.ExtentReports;

public sealed class FlushExtentReportsEventHandler : IEventHandler<AtataContextDeInitCompletedEvent>
{
    public void Handle(AtataContextDeInitCompletedEvent eventData, AtataContext context) =>
        ExtentContext.Reports.Flush();
}
