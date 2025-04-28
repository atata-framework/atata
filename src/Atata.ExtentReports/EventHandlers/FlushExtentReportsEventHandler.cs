namespace Atata.ExtentReports;

public sealed class FlushExtentReportsEventHandler : IEventHandler<AtataContextDeInitCompletedEvent>
{
    public void Handle(AtataContextDeInitCompletedEvent eventData, AtataContext context) =>
        context.Log.ExecuteSection(
            new LogSection("Flush ExtentReports report", LogLevel.Trace),
            ExtentContext.Reports.Flush);
}
