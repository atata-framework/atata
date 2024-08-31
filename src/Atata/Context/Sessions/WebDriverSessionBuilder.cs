namespace Atata;

public class WebDriverSessionBuilder : WebSessionBuilder<WebDriverSession, WebDriverSessionBuilder>
{
    /// <summary>
    /// Gets the screenshots configuration options.
    /// </summary>
    public ScreenshotsWebDriverSessionOptions Screenshots { get; private set; } = new();

    /// <summary>
    /// Gets the page snapshots configuration options.
    /// </summary>
    public PageSnapshotsWebDriverSessionOptions PageSnapshots { get; private set; } = new();

    protected override IScreenshotTaker CreateScreenshotTaker(WebDriverSession session) =>
        new ScreenshotTaker<WebDriverSession>(
            Screenshots.Strategy,
            WebDriverViewportScreenshotStrategy.Instance,
            FullPageOrViewportScreenshotStrategy.Instance,
            Screenshots.FileNameTemplate,
            session);

    protected override IPageSnapshotTaker CreatePageSnapshotTaker(WebDriverSession session) =>
        new PageSnapshotTaker<WebDriverSession>(
            PageSnapshots.Strategy,
            PageSnapshots.FileNameTemplate,
            session);

    protected override void OnClone(WebDriverSessionBuilder copy)
    {
        base.OnClone(copy);

        copy.Screenshots = Screenshots.Clone();
        copy.PageSnapshots = PageSnapshots.Clone();
    }
}
