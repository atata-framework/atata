namespace Atata;

[Obsolete("Use " + nameof(TakeScreenshotOnNUnitErrorEventHandler) + " instead.")] // Obsolete since v2.11.0.
public class TakeScreenshotOnNUnitErrorOnCleanUpEventHandler : IConditionalEventHandler<AtataContextCleanUpEvent>
{
    private readonly ScreenshotKind _screenshotKind;

    private readonly string _screenshotTitle;

    public TakeScreenshotOnNUnitErrorOnCleanUpEventHandler(string screenshotTitle = "Failed")
        : this(ScreenshotKind.Default, screenshotTitle)
    {
    }

    public TakeScreenshotOnNUnitErrorOnCleanUpEventHandler(ScreenshotKind screenshotKind, string screenshotTitle = "Failed")
    {
        _screenshotKind = screenshotKind;
        _screenshotTitle = screenshotTitle;
    }

    public bool CanHandle(AtataContextCleanUpEvent eventData, AtataContext context) =>
        NUnitAdapter.IsCurrentTestFailed() && context.HasDriver;

    public void Handle(AtataContextCleanUpEvent eventData, AtataContext context) =>
        context.TakeScreenshot(_screenshotKind, _screenshotTitle);
}
