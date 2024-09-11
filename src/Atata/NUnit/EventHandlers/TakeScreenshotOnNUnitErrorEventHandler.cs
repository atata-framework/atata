namespace Atata;

public sealed class TakeScreenshotOnNUnitErrorEventHandler : IConditionalEventHandler<AtataContextDeInitEvent>
{
    private readonly ScreenshotKind _screenshotKind;

    private readonly string _screenshotTitle;

    public TakeScreenshotOnNUnitErrorEventHandler(string screenshotTitle = "Failed")
        : this(ScreenshotKind.Default, screenshotTitle)
    {
    }

    public TakeScreenshotOnNUnitErrorEventHandler(ScreenshotKind screenshotKind, string screenshotTitle = "Failed")
    {
        _screenshotKind = screenshotKind;
        _screenshotTitle = screenshotTitle;
    }

    public bool CanHandle(AtataContextDeInitEvent eventData, AtataContext context) =>
        NUnitAdapter.IsCurrentTestFailed() && context.HasDriver;

    public void Handle(AtataContextDeInitEvent eventData, AtataContext context) =>
        context.TakeScreenshot(_screenshotKind, _screenshotTitle);
}
