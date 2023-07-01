using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Atata;

/// <summary>
/// Represents the builder of a screenshots configuration.
/// </summary>
public sealed class ScreenshotsAtataContextBuilder : AtataContextBuilder
{
    internal ScreenshotsAtataContextBuilder(AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
    }

    /// <summary>
    /// Sets the WebDriver viewport (<see cref="WebDriverViewportScreenshotStrategy"/>) strategy for a screenshot taking.
    /// </summary>
    /// <returns>The <see cref="ScreenshotsAtataContextBuilder"/> instance.</returns>
    public ScreenshotsAtataContextBuilder UseWebDriverViewportStrategy() =>
        UseStrategy(WebDriverViewportScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the WebDriver full-page (<see cref="WebDriverFullPageScreenshotStrategy"/>) strategy for a screenshot taking.
    /// Works only for <see cref="FirefoxDriver"/>.
    /// </summary>
    /// <returns>The <see cref="ScreenshotsAtataContextBuilder"/> instance.</returns>
    public ScreenshotsAtataContextBuilder UseWebDriverFullPageStrategy() =>
        UseStrategy(WebDriverFullPageScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the CDP full-page (<see cref="CdpFullPageScreenshotStrategy"/>) strategy for a screenshot taking.
    /// Works only for <see cref="ChromeDriver"/> and <see cref="EdgeDriver"/>.
    /// </summary>
    /// <returns>The <see cref="ScreenshotsAtataContextBuilder"/> instance.</returns>
    public ScreenshotsAtataContextBuilder UseCdpFullPageStrategy() =>
        UseStrategy(CdpFullPageScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the "full-page or viewport" (<see cref="FullPageOrViewportScreenshotStrategy"/>) strategy for a screenshot taking.
    /// </summary>
    /// <returns>The <see cref="ScreenshotsAtataContextBuilder"/> instance.</returns>
    public ScreenshotsAtataContextBuilder UseFullPageOrViewportStrategy() =>
        UseStrategy(FullPageOrViewportScreenshotStrategy.Instance);

    /// <summary>
    /// Sets the strategy for a screenshot taking.
    /// The default value is an instance of <see cref="WebDriverViewportScreenshotStrategy"/>.
    /// </summary>
    /// <param name="strategy">The screenshot strategy.</param>
    /// <returns>The <see cref="ScreenshotsAtataContextBuilder"/> instance.</returns>
    public ScreenshotsAtataContextBuilder UseStrategy(IScreenshotStrategy strategy)
    {
        BuildingContext.Screenshots.Strategy = strategy;
        return this;
    }
}
