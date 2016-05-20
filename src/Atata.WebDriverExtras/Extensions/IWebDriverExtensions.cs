using OpenQA.Selenium;
using System;
using System.Drawing;

namespace Atata
{
    public static class IWebDriverExtensions
    {
        public static T Maximize<T>(this T driver)
            where T : IWebDriver
        {
            driver.Manage().Window.Maximize();
            return driver;
        }

        public static T SetSize<T>(this T driver, int width, int height)
            where T : IWebDriver
        {
            driver.Manage().Window.Size = new Size(width, height);
            return driver;
        }

        public static T SetPosition<T>(this T driver, int x, int y)
            where T : IWebDriver
        {
            driver.Manage().Window.Position = new Point(x, y);
            return driver;
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver)
        {
            return new WebDriverExtendedSearchContext(driver);
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver, TimeSpan timeout)
        {
            return new WebDriverExtendedSearchContext(driver, timeout);
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver, TimeSpan timeout, TimeSpan retryInterval)
        {
            return new WebDriverExtendedSearchContext(driver, timeout, retryInterval);
        }

        public static bool TitleContains(this IWebDriver driver, string text)
        {
            return driver.Title != null ? driver.Title.Contains(text) : false;
        }
    }
}
