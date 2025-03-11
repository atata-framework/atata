#nullable enable

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Atata;

/// <summary>
/// Represents a <see cref="WebDriverSession"/> screenshot strategy that tries to take a full-page screenshot,
/// when it cannot be taken, takes a screenshot of viewport area.
/// </summary>
public sealed class FullPageOrViewportScreenshotStrategy : IScreenshotStrategy<WebDriverSession>
{
    private static readonly ConcurrentDictionary<string, bool> s_driverAliasSupportsCdpMap = new();

    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static FullPageOrViewportScreenshotStrategy Instance { get; } = new();

    /// <inheritdoc/>
    public FileContentWithExtension TakeScreenshot(WebDriverSession session)
    {
        var driver = session.Driver;

        if (driver.Is<FirefoxDriver>())
            return WebDriverFullPageScreenshotStrategy.Instance.TakeScreenshot(session);

        string? driverAlias = session.DriverAlias;

        if (string.IsNullOrEmpty(driverAlias))
            driverAlias = driver.GetType().Name;

        if (!s_driverAliasSupportsCdpMap.TryGetValue(driverAlias, out bool isCdpSupported))
        {
            isCdpSupported = driver.Is<ChromeDriver>()
                || driver.Is<EdgeDriver>()
                || driver.Is<RemoteWebDriver>();

            s_driverAliasSupportsCdpMap[driverAlias] = isCdpSupported;
        }

        if (isCdpSupported)
        {
            try
            {
                return CdpFullPageScreenshotStrategy.Instance.TakeScreenshot(session);
            }
            catch (Exception exception)
            {
                s_driverAliasSupportsCdpMap[driverAlias] = false;

                session.Log.Warn(exception, $"Failed to take a full-page screenshot via CDP. {nameof(ITakesScreenshot)}.{nameof(ITakesScreenshot.GetScreenshot)} will be used to take screenshot.");
            }
        }

        return WebDriverViewportScreenshotStrategy.Instance.TakeScreenshot(session);
    }
}
