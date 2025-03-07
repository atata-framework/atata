using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Atata;

/// <summary>
/// Represents a configuration builder of page screenshots functionality for <see cref="WebDriverSession"/>.
/// </summary>
public sealed class ScreenshotsWebDriverSessionBuilder : ICloneable
{
    /// <summary>
    /// Gets or sets the strategy for a page screenshot taking.
    /// The default value is an instance of <see cref="WebDriverViewportScreenshotStrategy"/>.
    /// </summary>
    public IScreenshotStrategy<WebDriverSession> Strategy { get; set; } =
        WebDriverViewportScreenshotStrategy.Instance;

    /// <summary>
    /// Gets or sets the page screenshot file name template.
    /// The file name is relative to Artifacts path.
    /// The default value is <c>"{session-id}-{screenshot-number:D2}{screenshot-pageobjectname: *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}"</c>.
    /// </summary>
    public string FileNameTemplate { get; set; } =
        "{session-id}-{screenshot-number:D2}{screenshot-pageobjectname: *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}";

    /// <summary>
    /// Gets or sets a value indicating whether to take a screenshot on failure.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool TakeOnFailure { get; set; } = true;

    /// <summary>
    /// Sets the WebDriver viewport (<see cref="WebDriverViewportScreenshotStrategy"/>) strategy for a screenshot taking.
    /// </summary>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionBuilder"/> instance.</returns>
    public ScreenshotsWebDriverSessionBuilder UseWebDriverViewportStrategy() =>
        UseStrategy(WebDriverViewportScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the WebDriver full-page (<see cref="WebDriverFullPageScreenshotStrategy"/>) strategy for a screenshot taking.
    /// Works only for <see cref="FirefoxDriver"/>.
    /// </summary>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionBuilder"/> instance.</returns>
    public ScreenshotsWebDriverSessionBuilder UseWebDriverFullPageStrategy() =>
        UseStrategy(WebDriverFullPageScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the CDP full-page (<see cref="CdpFullPageScreenshotStrategy"/>) strategy for a screenshot taking.
    /// Works only for <see cref="ChromeDriver"/> and <see cref="EdgeDriver"/>.
    /// </summary>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionBuilder"/> instance.</returns>
    public ScreenshotsWebDriverSessionBuilder UseCdpFullPageStrategy() =>
        UseStrategy(CdpFullPageScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the "full-page or viewport" (<see cref="FullPageOrViewportScreenshotStrategy"/>) strategy for a screenshot taking.
    /// </summary>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionBuilder"/> instance.</returns>
    public ScreenshotsWebDriverSessionBuilder UseFullPageOrViewportStrategy() =>
        UseStrategy(FullPageOrViewportScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the strategy for a screenshot taking.
    /// The default value is an instance of <see cref="WebDriverViewportScreenshotStrategy"/>.
    /// </summary>
    /// <param name="strategy">The screenshot strategy.</param>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionBuilder"/> instance.</returns>
    public ScreenshotsWebDriverSessionBuilder UseStrategy(IScreenshotStrategy<WebDriverSession> strategy)
    {
        Strategy = strategy;
        return this;
    }

    /// <summary>
    /// Sets the file name template of page screenshots.
    /// The default value is <c>"{session-id}-{screenshot-number:D2}{screenshot-pageobjectname: *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}"</c>.
    /// </summary>
    /// <param name="fileNameTemplate">The file name template.</param>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionBuilder"/> instance.</returns>
    public ScreenshotsWebDriverSessionBuilder UseFileNameTemplate(string fileNameTemplate)
    {
        FileNameTemplate = fileNameTemplate;
        return this;
    }

    /// <summary>
    /// Sets a value indicating whether to take a screenshot on failure.
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="enable">Whether to enable.</param>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionBuilder"/> instance.</returns>
    public ScreenshotsWebDriverSessionBuilder UseTakeOnFailure(bool enable)
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
    internal ScreenshotsWebDriverSessionBuilder Clone()
    {
        var clone = (ScreenshotsWebDriverSessionBuilder)MemberwiseClone();

        if (Strategy is ICloneable cloneableScreenshotStrategy)
            clone.Strategy = (IScreenshotStrategy<WebDriverSession>)cloneableScreenshotStrategy.Clone();

        return clone;
    }
}
