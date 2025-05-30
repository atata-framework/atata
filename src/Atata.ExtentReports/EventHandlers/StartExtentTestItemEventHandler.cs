namespace Atata.ExtentReports;

public sealed class StartExtentTestItemEventHandler : IEventHandler<AtataContextPreInitEvent>
{
    public void Handle(AtataContextPreInitEvent eventData, AtataContext context)
    {
        var extentContext = ExtentContext.ResolveFor(context);

        if (context.Test.Traits.Count > 0)
        {
            string[] categories = [.. context.Test.Traits.Select(x => x.ToString())];

            extentContext.Test.AssignCategory(categories);
        }
    }
}
