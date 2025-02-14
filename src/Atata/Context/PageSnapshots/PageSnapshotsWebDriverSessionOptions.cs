namespace Atata;

/// <summary>
/// Represents a configuration of page snapshots functionality for <see cref="WebDriverSession"/>.
/// </summary>
public sealed class PageSnapshotsWebDriverSessionOptions : ICloneable
{
    /// <summary>
    /// Gets or sets the strategy for a page snapshot taking.
    /// The default value is an instance of <see cref="CdpOrPageSourcePageSnapshotStrategy"/>.
    /// </summary>
    public IPageSnapshotStrategy<WebDriverSession> Strategy { get; set; } =
        CdpOrPageSourcePageSnapshotStrategy.Instance;

    /// <summary>
    /// Gets or sets the page snapshot file name template.
    /// The file name is relative to Artifacts path.
    /// The default value is <c>"{session-id}-{snapshot-number:D2}{snapshot-pageobjectname: *}{snapshot-pageobjecttypename: *}{snapshot-title: - *}"</c>.
    /// </summary>
    public string FileNameTemplate { get; set; } =
        "{session-id}-{snapshot-number:D2}{snapshot-pageobjectname: *}{snapshot-pageobjecttypename: *}{snapshot-title: - *}";

    /// <summary>
    /// Gets or sets a value indicating whether to take a page snapshot on failure.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool TakeOnFailure { get; set; } = true;

    /// <summary>
    /// Sets the "CDP or page source" (<see cref="CdpOrPageSourcePageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The same <see cref="PageSnapshotsWebDriverSessionOptions"/> instance.</returns>
    public PageSnapshotsWebDriverSessionOptions UseCdpOrPageSourceStrategy() =>
        UseStrategy(CdpOrPageSourcePageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the page source (<see cref="PageSourcePageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The same <see cref="PageSnapshotsWebDriverSessionOptions"/> instance.</returns>
    public PageSnapshotsWebDriverSessionOptions UsePageSourceStrategy() =>
        UseStrategy(PageSourcePageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the CDP (<see cref="CdpPageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The same <see cref="PageSnapshotsWebDriverSessionOptions"/> instance.</returns>
    public PageSnapshotsWebDriverSessionOptions UseCdpStrategy() =>
        UseStrategy(CdpPageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the strategy for a page snapshot taking.
    /// The default value is an instance of <see cref="CdpOrPageSourcePageSnapshotStrategy"/>.
    /// </summary>
    /// <param name="strategy">The snapshot take strategy.</param>
    /// <returns>The same <see cref="PageSnapshotsWebDriverSessionOptions"/> instance.</returns>
    public PageSnapshotsWebDriverSessionOptions UseStrategy(IPageSnapshotStrategy<WebDriverSession> strategy)
    {
        Strategy = strategy;
        return this;
    }

    /// <summary>
    /// Sets the file name template of page snapshots.
    /// The default value is <c>"{session-id}-{snapshot-number:D2}{snapshot-pageobjectname: *}{snapshot-pageobjecttypename: *}{snapshot-title: - *}"</c>.
    /// </summary>
    /// <param name="fileNameTemplate">The file name template.</param>
    /// <returns>The same <see cref="PageSnapshotsWebDriverSessionOptions"/> instance.</returns>
    public PageSnapshotsWebDriverSessionOptions UseFileNameTemplate(string fileNameTemplate)
    {
        FileNameTemplate = fileNameTemplate;
        return this;
    }

    /// <summary>
    /// Sets a value indicating whether to take a page snapshot on failure.
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="enable">Whether to enable.</param>
    /// <returns>The same <see cref="PageSnapshotsWebDriverSessionOptions"/> instance.</returns>
    public PageSnapshotsWebDriverSessionOptions UseTakeOnFailure(bool enable)
    {
        TakeOnFailure = enable;
        return this;
    }

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    internal PageSnapshotsWebDriverSessionOptions Clone()
    {
        var clone = (PageSnapshotsWebDriverSessionOptions)MemberwiseClone();

        if (Strategy is ICloneable cloneablePageSnapshotStrategy)
            clone.Strategy = (IPageSnapshotStrategy<WebDriverSession>)cloneablePageSnapshotStrategy.Clone();

        return clone;
    }
}
