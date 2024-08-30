using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Atata;

/// <summary>
/// Represents a configuration of page screenshots functionality for <see cref="WebDriverSession"/>.
/// </summary>
public sealed class ScreenshotsWebDriverSessionOptions : ICloneable
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
    /// The default value is <c>"{screenshot-number:D2}{screenshot-pageobjectname: *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}"</c>.
    /// </summary>
    public string FileNameTemplate { get; set; } =
        "{screenshot-number:D2}{screenshot-pageobjectname: *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}";

    /// <summary>
    /// Sets the WebDriver viewport (<see cref="WebDriverViewportScreenshotStrategy"/>) strategy for a screenshot taking.
    /// </summary>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionOptions"/> instance.</returns>
    public ScreenshotsWebDriverSessionOptions UseWebDriverViewportStrategy() =>
        UseStrategy(WebDriverViewportScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the WebDriver full-page (<see cref="WebDriverFullPageScreenshotStrategy"/>) strategy for a screenshot taking.
    /// Works only for <see cref="FirefoxDriver"/>.
    /// </summary>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionOptions"/> instance.</returns>
    public ScreenshotsWebDriverSessionOptions UseWebDriverFullPageStrategy() =>
        UseStrategy(WebDriverFullPageScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the CDP full-page (<see cref="CdpFullPageScreenshotStrategy"/>) strategy for a screenshot taking.
    /// Works only for <see cref="ChromeDriver"/> and <see cref="EdgeDriver"/>.
    /// </summary>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionOptions"/> instance.</returns>
    public ScreenshotsWebDriverSessionOptions UseCdpFullPageStrategy() =>
        UseStrategy(CdpFullPageScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the "full-page or viewport" (<see cref="FullPageOrViewportScreenshotStrategy"/>) strategy for a screenshot taking.
    /// </summary>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionOptions"/> instance.</returns>
    public ScreenshotsWebDriverSessionOptions UseFullPageOrViewportStrategy() =>
        UseStrategy(FullPageOrViewportScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the strategy for a screenshot taking.
    /// The default value is an instance of <see cref="WebDriverViewportScreenshotStrategy"/>.
    /// </summary>
    /// <param name="strategy">The screenshot strategy.</param>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionOptions"/> instance.</returns>
    public ScreenshotsWebDriverSessionOptions UseStrategy(IScreenshotStrategy<WebDriverSession> strategy)
    {
        Strategy = strategy;
        return this;
    }

    /// <summary>
    /// Sets the file name template of page screenshots.
    /// The default value is <c>"{screenshot-number:D2}{screenshot-pageobjectname: *}{screenshot-pageobjecttypename: *}{screenshot-title: - *}"</c>.
    /// </summary>
    /// <param name="fileNameTemplate">The file name template.</param>
    /// <returns>The same <see cref="ScreenshotsWebDriverSessionOptions"/> instance.</returns>
    public ScreenshotsWebDriverSessionOptions UseFileNameTemplate(string fileNameTemplate)
    {
        FileNameTemplate = fileNameTemplate;
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
    internal ScreenshotsWebDriverSessionOptions Clone()
    {
        var clone = (ScreenshotsWebDriverSessionOptions)MemberwiseClone();

        if (Strategy is ICloneable cloneableScreenshotStrategy)
            clone.Strategy = (IScreenshotStrategy<WebDriverSession>)cloneableScreenshotStrategy.Clone();

        return clone;
    }
}
