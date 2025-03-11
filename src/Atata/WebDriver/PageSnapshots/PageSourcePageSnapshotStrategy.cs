#nullable enable

namespace Atata;

/// <summary>
/// Represents a <see cref="WebDriverSession"/> page snapshot strategy that takes a page snapshot using <see cref="IWebDriver.PageSource"/>.
/// </summary>
public sealed class PageSourcePageSnapshotStrategy : IPageSnapshotStrategy<WebDriverSession>
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static PageSourcePageSnapshotStrategy Instance { get; } = new();

    /// <inheritdoc/>
    public FileContentWithExtension TakeSnapshot(WebDriverSession session)
    {
        var content = session.Driver.PageSource;
        return FileContentWithExtension.CreateFromText(content, ".html");
    }
}
