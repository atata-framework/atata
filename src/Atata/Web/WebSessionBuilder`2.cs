namespace Atata;

public abstract class WebSessionBuilder<TSession, TBuilder> : AtataSessionBuilder<TSession, TBuilder>
    where TSession : WebSession, new()
    where TBuilder : WebSessionBuilder<TSession, TBuilder>
{
    /// <summary>
    /// Gets or sets the element find timeout for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="AtataSessionBuilder{TSession, TBuilder}.BaseRetryTimeout"/>  or <see cref="AtataContext.BaseRetryTimeout"/>,
    /// which are equal to <c>5</c> seconds by default.
    /// </summary>
    public TimeSpan? ElementFindTimeout { get; set; }

    /// <summary>
    /// Gets or sets the element find retry interval for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="AtataSessionBuilder{TSession, TBuilder}.BaseRetryInterval"/> or <see cref="AtataContext.BaseRetryInterval"/>,
    /// which are equal to <c>500</c> milliseconds by default.
    /// </summary>
    public TimeSpan? ElementFindRetryInterval { get; set; }

    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the name of the DOM test identifier attribute.
    /// The default value is <c>"data-testid"</c>.
    /// </summary>
    public string DomTestIdAttributeName { get; set; } = FindByTestIdAttribute.DefaultAttributeName;

    /// <summary>
    /// Gets or sets the default case of the DOM test identifier attribute.
    /// The default value is <see cref="TermCase.Kebab"/>.
    /// </summary>
    public TermCase DomTestIdAttributeDefaultCase { get; set; } = FindByTestIdAttribute.DefaultAttributeCase;

    /// <summary>
    /// Sets the base URL.
    /// </summary>
    /// <param name="baseUrl">The base URL.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseBaseUrl(string baseUrl)
    {
        if (baseUrl != null && !Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            throw new ArgumentException($"Invalid URL format \"{baseUrl}\".", nameof(baseUrl));

        BaseUrl = baseUrl;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="ElementFindTimeout"/> value.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseElementFindTimeout(TimeSpan? timeout)
    {
        ElementFindTimeout = timeout;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="ElementFindRetryInterval"/> value.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseElementFindRetryInterval(TimeSpan? interval)
    {
        ElementFindRetryInterval = interval;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the name of the DOM test identifier attribute.
    /// The default value is <c>"data-testid"</c>.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseDomTestIdAttributeName(string name)
    {
        name.CheckNotNullOrWhitespace(nameof(name));
        DomTestIdAttributeName = name;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the default case of the DOM test identifier attribute.
    /// The default value is <see cref="TermCase.Kebab"/>.
    /// </summary>
    /// <param name="defaultCase">The default case.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseDomTestIdAttributeDefaultCase(TermCase defaultCase)
    {
        DomTestIdAttributeDefaultCase = defaultCase;

        return (TBuilder)this;
    }

    protected override void ConfigureSession(TSession session, AtataContext context)
    {
        session.BaseUrl = BaseUrl;
        session.ElementFindTimeoutOptional = ElementFindTimeout;
        session.ElementFindRetryIntervalOptional = ElementFindRetryInterval;

        session.ScreenshotTaker = CreateScreenshotTaker(session);
        session.PageSnapshotTaker = CreatePageSnapshotTaker(session);
    }

    protected abstract IScreenshotTaker CreateScreenshotTaker(TSession session);

    protected abstract IPageSnapshotTaker CreatePageSnapshotTaker(TSession session);
}
