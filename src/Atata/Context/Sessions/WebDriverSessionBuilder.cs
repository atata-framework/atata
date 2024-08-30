namespace Atata;

public class WebDriverSessionBuilder : AtataSessionBuilder<WebDriverSessionBuilder>
{
    /// <summary>
    /// Gets or sets the element find timeout for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="AtataSessionBuilder{TBuilder}.BaseRetryTimeout"/>  or <see cref="AtataContext.BaseRetryTimeout"/>,
    /// which are equal to <c>5</c> seconds by default.
    /// </summary>
    public TimeSpan? ElementFindTimeout { get; set; }

    /// <summary>
    /// Gets or sets the element find retry interval for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="AtataSessionBuilder{TBuilder}.BaseRetryInterval"/> or <see cref="AtataContext.BaseRetryInterval"/>,
    /// which are equal to <c>500</c> milliseconds by default.
    /// </summary>
    public TimeSpan? ElementFindRetryInterval { get; set; }

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

    /// <summary>
    /// Sets the <see cref="ElementFindTimeout"/> value.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseElementFindTimeout(TimeSpan? timeout)
    {
        ElementFindTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="ElementFindRetryInterval"/> value.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseElementFindRetryInterval(TimeSpan? interval)
    {
        ElementFindRetryInterval = interval;
        return this;
    }

    protected override AtataSession Create(AtataContext context)
    {
        WebDriverSession session = new(context)
        {
            BaseUrl = BaseUrl,
            ElementFindTimeout = ElementFindTimeout ?? BaseRetryTimeout ?? context.BaseRetryTimeout,
            ElementFindRetryInterval = ElementFindRetryInterval ?? BaseRetryTimeout ?? context.BaseRetryTimeout
        };

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

        return session;
    }

    protected override void OnClone(WebDriverSessionBuilder copy)
    {
        base.OnClone(copy);

        copy.Screenshots = Screenshots.Clone();
        copy.PageSnapshots = PageSnapshots.Clone();
    }
}
