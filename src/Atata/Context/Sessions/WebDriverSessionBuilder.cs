namespace Atata;

public class WebDriverSessionBuilder : WebSessionBuilder<WebDriverSession, WebDriverSessionBuilder>
{
    /// <summary>
    /// Gets or sets the default control visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    public Visibility DefaultControlVisibility { get; set; }

    /// <summary>
    /// Gets the screenshots configuration options.
    /// </summary>
    public ScreenshotsWebDriverSessionOptions Screenshots { get; private set; } = new();

    /// <summary>
    /// Gets the page snapshots configuration options.
    /// </summary>
    public PageSnapshotsWebDriverSessionOptions PageSnapshots { get; private set; } = new();

    /// <summary>
    /// Sets the default control visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    /// <param name="visibility">The visibility.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDefaultControlVisibility(Visibility visibility)
    {
        DefaultControlVisibility = visibility;
        return this;
    }

    protected override void ConfigureSession(WebDriverSession session, AtataContext context)
    {
        base.ConfigureSession(session, context);

        session.DefaultControlVisibility = DefaultControlVisibility;
    }

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
