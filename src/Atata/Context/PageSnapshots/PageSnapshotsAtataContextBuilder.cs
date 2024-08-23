namespace Atata;

/// <summary>
/// Represents the builder of a page snapshots configuration.
/// </summary>
public sealed class PageSnapshotsAtataContextBuilder : AtataContextBuilder
{
    internal PageSnapshotsAtataContextBuilder(AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
    }

    /// <summary>
    /// Sets the "CDP or page source" (<see cref="CdpOrPageSourcePageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The <see cref="PageSnapshotsAtataContextBuilder"/> instance.</returns>
    public PageSnapshotsAtataContextBuilder UseCdpOrPageSourceStrategy() =>
        UseStrategy(CdpOrPageSourcePageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the page source (<see cref="PageSourcePageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The <see cref="PageSnapshotsAtataContextBuilder"/> instance.</returns>
    public PageSnapshotsAtataContextBuilder UsePageSourceStrategy() =>
        UseStrategy(PageSourcePageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the CDP (<see cref="CdpPageSnapshotStrategy"/>) strategy for a page snapshot taking.
    /// </summary>
    /// <returns>The <see cref="PageSnapshotsAtataContextBuilder"/> instance.</returns>
    public PageSnapshotsAtataContextBuilder UseCdpStrategy() =>
        UseStrategy(CdpPageSnapshotStrategy.Instance);

    /// <summary>
    /// Sets the strategy for a page snapshot taking.
    /// The default value is an instance of <see cref="CdpOrPageSourcePageSnapshotStrategy"/>.
    /// </summary>
    /// <param name="strategy">The snapshot take strategy.</param>
    /// <returns>The <see cref="PageSnapshotsAtataContextBuilder"/> instance.</returns>
    public PageSnapshotsAtataContextBuilder UseStrategy(IPageSnapshotStrategy<WebDriverSession> strategy)
    {
        BuildingContext.PageSnapshots.Strategy = strategy;
        return this;
    }

    /// <summary>
    /// Sets the file name template of page snapshots.
    /// The default value is <c>"{snapshot-number:D2}{snapshot-pageobjectname: *}{snapshot-pageobjecttypename: *}{snapshot-title: - *}"</c>.
    /// </summary>
    /// <param name="fileNameTemplate">The file name template.</param>
    /// <returns>The <see cref="PageSnapshotsAtataContextBuilder"/> instance.</returns>
    public PageSnapshotsAtataContextBuilder UseFileNameTemplate(string fileNameTemplate)
    {
        BuildingContext.PageSnapshots.FileNameTemplate = fileNameTemplate;
        return this;
    }
}
