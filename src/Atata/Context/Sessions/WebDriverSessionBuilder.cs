namespace Atata;

public class WebDriverSessionBuilder : AtataSessionBuilder<WebDriverSessionBuilder>
{
    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Gets the screenshots configuration options.
    /// </summary>
    public ScreenshotsWebDriverSessionOptions Screenshots { get; private set; } = new();

    /// <summary>
    /// Gets the page snapshots configuration options.
    /// </summary>
    public PageSnapshotsWebDriverSessionOptions PageSnapshots { get; private set; } = new();

    /// <summary>
    /// Sets the base URL.
    /// </summary>
    /// <param name="baseUrl">The base URL.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseBaseUrl(string baseUrl)
    {
        if (baseUrl != null && !Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            throw new ArgumentException($"Invalid URL format \"{baseUrl}\".", nameof(baseUrl));

        BaseUrl = baseUrl;
        return this;
    }

    public override AtataSession Build(AtataContext context)
    {
        WebDriverSession session = new(context);

        session.BaseUrl = BaseUrl;

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
