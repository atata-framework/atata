namespace Atata.ExtentReports;

public sealed class StartExtentTestItemEventHandler : IEventHandler<AtataContextPreInitEvent>
{
    public void Handle(AtataContextPreInitEvent eventData, AtataContext context) =>
        ExtentContext.ResolveFor(context);
}
