namespace Atata;

/// <summary>
/// Represents the strategy that takes a page snapshot using <see cref="IWebDriver.PageSource"/>.
/// </summary>
public sealed class PageSourcePageSnapshotStrategy : IPageSnapshotStrategy
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static PageSourcePageSnapshotStrategy Instance { get; } =
        new PageSourcePageSnapshotStrategy();

    /// <inheritdoc/>
    public FileContentWithExtension TakeSnapshot(WebDriverSession session)
    {
        var content = session.Driver.PageSource;
        return FileContentWithExtension.CreateFromText(content, ".html");
    }
}
