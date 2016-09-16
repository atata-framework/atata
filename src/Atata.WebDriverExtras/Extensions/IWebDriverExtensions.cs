using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    public static class IWebDriverExtensions
    {
        public static T Maximize<T>(this T driver)
            where T : IWebDriver
        {
            driver.CheckNotNull(nameof(driver));

            driver.Manage().Window.Maximize();
            return driver;
        }

        public static T SetSize<T>(this T driver, int width, int height)
            where T : IWebDriver
        {
            driver.CheckNotNull(nameof(driver));

            driver.Manage().Window.Size = new Size(width, height);
            return driver;
        }

        public static T SetPosition<T>(this T driver, int x, int y)
            where T : IWebDriver
        {
            driver.CheckNotNull(nameof(driver));

            driver.Manage().Window.Position = new Point(x, y);
            return driver;
        }

        public static T Perform<T>(this T driver, Func<Actions, Actions> actionsBuilder)
            where T : IWebDriver
        {
            driver.CheckNotNull(nameof(driver));
            actionsBuilder.CheckNotNull(nameof(actionsBuilder));

            Actions actions = new Actions(driver);
            actions = actionsBuilder(actions);
            actions.Perform();

            return driver;
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver)
        {
            driver.CheckNotNull(nameof(driver));

            return new WebDriverExtendedSearchContext(driver);
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver, TimeSpan timeout)
        {
            driver.CheckNotNull(nameof(driver));

            return new WebDriverExtendedSearchContext(driver, timeout);
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver, TimeSpan timeout, TimeSpan retryInterval)
        {
            driver.CheckNotNull(nameof(driver));

            return new WebDriverExtendedSearchContext(driver, timeout, retryInterval);
        }

        public static bool TitleContains(this IWebDriver driver, string text)
        {
            driver.CheckNotNull(nameof(driver));
            text.CheckNotNullOrEmpty(nameof(text));

            return driver?.Title.Contains(text) ?? false;
        }
    }
}
