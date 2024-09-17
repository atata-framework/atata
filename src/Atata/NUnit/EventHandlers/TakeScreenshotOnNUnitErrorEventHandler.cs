namespace Atata;

public sealed class TakeScreenshotOnNUnitErrorEventHandler : IConditionalEventHandler<AtataSessionDeInitEvent>
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

    public bool CanHandle(AtataSessionDeInitEvent eventData, AtataContext context) =>
        NUnitAdapter.IsCurrentTestFailed() && eventData.Session.IsActive;

    public void Handle(AtataSessionDeInitEvent eventData, AtataContext context) =>
        (eventData.Session as WebSession)?.TakeScreenshot(_screenshotKind, _screenshotTitle);
}
