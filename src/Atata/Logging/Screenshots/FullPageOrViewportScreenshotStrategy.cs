using System;
using System.Collections.Concurrent;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Atata
{
    /// <summary>
    /// Represents the strategy that tries to take a full-page screenshot,
    /// when it cannot be taken, takes a screenshot of viewport area.
    /// </summary>
    public sealed class FullPageOrViewportScreenshotStrategy : IScreenshotStrategy
    {
        private static readonly ConcurrentDictionary<string, bool> s_driverAliasSupportsCdpMap =
            new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static FullPageOrViewportScreenshotStrategy Instance { get; } =
            new FullPageOrViewportScreenshotStrategy();

        /// <inheritdoc/>
        public FileContentWithExtension TakeScreenshot(AtataContext context)
        {
            var driver = context.Driver;

            if (driver.Is<FirefoxDriver>())
                return WebDriverFullPageScreenshotStrategy.Instance.TakeScreenshot(context);

            string driverAlias = context.DriverAlias;

            if (string.IsNullOrEmpty(driverAlias))
                driverAlias = context.Driver.GetType().Name;

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
                    return CdpFullPageScreenshotStrategy.Instance.TakeScreenshot(context);
                }
                catch (Exception exception)
                {
                    s_driverAliasSupportsCdpMap[driverAlias] = false;

                    context.Log.Warn($"Failed to take a full-page screenshot via CDP. {nameof(ITakesScreenshot)}.{nameof(ITakesScreenshot.GetScreenshot)} will be used to take screenshot.", exception);
                }
            }

            return WebDriverViewportScreenshotStrategy.Instance.TakeScreenshot(context);
        }
    }
}
