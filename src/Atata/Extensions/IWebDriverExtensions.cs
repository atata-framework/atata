using OpenQA.Selenium;
using System;

namespace Atata
{
    public static class IWebDriverExtensions
    {
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

        public static bool Exists(this IWebDriver element, By by)
        {
            return new WebDriverExtendedSearchContext(element).Exists(by);
        }

        public static bool Missing(this IWebDriver element, By by)
        {
            return new WebDriverExtendedSearchContext(element).Missing(by);
        }
    }
}
