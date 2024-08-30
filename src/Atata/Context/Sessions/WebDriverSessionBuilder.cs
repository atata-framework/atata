namespace Atata;

public class WebDriverSessionBuilder : AtataSessionBuilder<WebDriverSessionBuilder>
{
    /// <summary>
    /// Gets the screenshots configuration options.
    /// </summary>
    public ScreenshotsWebDriverSessionOptions Screenshots { get; private set; } = new();

    /// <summary>
    /// Gets the page snapshots configuration options.
    /// </summary>
    public PageSnapshotsWebDriverSessionOptions PageSnapshots { get; private set; } = new();

    public override AtataSession Build(AtataContext context)
    {
        WebDriverSession session = new(context);

        session.ScreenshotTaker = new(
            Screenshots.Strategy,
            WebDriverViewportScreenshotStrategy.Instance,
            FullPageOrViewportScreenshotStrategy.Instance,
            Screenshots.FileNameTemplate,
            session);

        session.PageSnapshotTaker = new PageSnapshotTaker<WebDriverSession>(
            PageSnapshots.Strategy,
            PageSnapshots.FileNameTemplate,
            session);

        session.Start();

        return session;
    }

    protected override void OnClone(WebDriverSessionBuilder copy)
    {
        base.OnClone(copy);

        copy.Screenshots = Screenshots.Clone();
        copy.PageSnapshots = PageSnapshots.Clone();
    }
}
