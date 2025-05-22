namespace Atata;

/// <summary>
/// Represents a configuration builder of page snapshots functionality for <see cref="WebDriverSession"/>.
/// </summary>
public sealed class PageSnapshotsWebDriverSessionBuilder
{
    /// <summary>
    /// The default file name template without session identifier.
    /// </summary>
    public const string DefaultFileNameTemplateWithoutSessionId =
        "{snapshot-pageobjectname}{snapshot-pageobjecttypename:_*}{snapshot-title:-*}";

    /// <summary>
    /// The default file name template with session identifier.
    /// </summary>
    public const string DefaultFileNameTemplateWithSessionId =
        "{session-id}-" + DefaultFileNameTemplateWithoutSessionId;

    private readonly WebDriverSessionBuilder _sessionBuilder;

    internal PageSnapshotsWebDriverSessionBuilder(WebDriverSessionBuilder sessionBuilder) =>
        _sessionBuilder = sessionBuilder;

    /// <summary>
    /// Gets or sets the strategy for a page snapshot taking.
    /// The default value is an instance of <see cref="CdpOrPageSourcePageSnapshotStrategy"/>.
    /// </summary>
    public IPageSnapshotStrategy<WebDriverSession> Strategy { get; set; } =
        CdpOrPageSourcePageSnapshotStrategy.Instance;

    /// <summary>
    /// Gets or sets the page snapshot file name template.
    /// The file name is relative to Artifacts path.
    /// The default value is <c>"{snapshot-pageobjectname}{snapshot-pageobjecttypename:_*}{snapshot-title:-*}"</c>.
    /// </summary>
    public string FileNameTemplate { get; set; } =
        DefaultFileNameTemplateWithoutSessionId;

    /// <summary>
    /// Gets or sets a value indicating whether to prepend artifact number to file name in a form of "001-{file name}".
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool PrependArtifactNumberToFileName { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to take a page snapshot on failure.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool TakeOnFailure { get; set; } = true;

    /// <summary>
    /// Sets the "CDP or page source" (<see cref="CdpOrPageSourcePageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseCdpOrPageSourceStrategy() =>
        UseStrategy(CdpOrPageSourcePageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the page source (<see cref="PageSourcePageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UsePageSourceStrategy() =>
        UseStrategy(PageSourcePageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the CDP (<see cref="CdpPageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseCdpStrategy() =>
        UseStrategy(CdpPageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the strategy for a page snapshot taking.
    /// The default value is an instance of <see cref="CdpOrPageSourcePageSnapshotStrategy"/>.
    /// </summary>
    /// <param name="strategy">The snapshot take strategy.</param>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseStrategy(IPageSnapshotStrategy<WebDriverSession> strategy)
    {
        Strategy = strategy;
        return _sessionBuilder;
    }

    /// <summary>
    /// Sets the file name template of page snapshots.
    /// The default value is <c>"{snapshot-pageobjectname}{snapshot-pageobjecttypename:_*}{snapshot-title:-*}"</c>.
    /// </summary>
    /// <param name="fileNameTemplate">The file name template.</param>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseFileNameTemplate(string fileNameTemplate)
    {
        FileNameTemplate = fileNameTemplate;
        return _sessionBuilder;
    }

    /// <summary>
    /// Sets the file name template of page snapshots including <c>{session-id}</c> variable.
    /// The set value is <c>"{session-id}-{snapshot-pageobjectname}{snapshot-pageobjecttypename:_*}{snapshot-title:-*}"</c>.
    /// </summary>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseFileNameTemplateWithSessionId() =>
        UseFileNameTemplate(DefaultFileNameTemplateWithSessionId);

    /// <summary>
    /// Sets a value indicating whether to prepend artifact number to file name in a form of "001-{file name}".
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="enable">Whether to enable.</param>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UsePrependArtifactNumberToFileName(bool enable)
    {
        PrependArtifactNumberToFileName = enable;
        return _sessionBuilder;
    }

    /// <summary>
    /// Sets a value indicating whether to take a page snapshot on failure.
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="enable">Whether to enable.</param>
    /// <returns>The <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseTakeOnFailure(bool enable)
    {
        TakeOnFailure = enable;
        return _sessionBuilder;
    }

    internal PageSnapshotsWebDriverSessionBuilder CloneFor(WebDriverSessionBuilder sessionBuilder) =>
        new(sessionBuilder)
        {
            Strategy = Strategy is ICloneable cloneablePageSnapshotStrategy
                ? (IPageSnapshotStrategy<WebDriverSession>)cloneablePageSnapshotStrategy.Clone()
                : Strategy,
            FileNameTemplate = FileNameTemplate,
            TakeOnFailure = TakeOnFailure
        };
}
