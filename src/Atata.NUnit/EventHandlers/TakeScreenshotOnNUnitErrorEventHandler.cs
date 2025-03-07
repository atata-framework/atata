namespace Atata.NUnit;

[Obsolete("Don't use this class, as it will be removed. " +
    "It is recommended to inherit AtataGlobalFixture and AtataTestSuite classes for global and test suites. " +
    "Otherwise use AtataContext.HandleTestResultException(...) method on test tear down.")] // Obsolete since v4.0.0.
public sealed class TakeScreenshotOnNUnitErrorEventHandler : IConditionalEventHandler<AtataSessionDeInitStartedEvent>
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

    public bool CanHandle(AtataSessionDeInitStartedEvent eventData, AtataContext context) =>
        TestContext.CurrentContext.Result.Outcome.IsCurrentTestFailed() && eventData.Session.IsActive;

    public void Handle(AtataSessionDeInitStartedEvent eventData, AtataContext context) =>
        (eventData.Session as WebSession)?.TakeScreenshot(_screenshotKind, _screenshotTitle);
}
