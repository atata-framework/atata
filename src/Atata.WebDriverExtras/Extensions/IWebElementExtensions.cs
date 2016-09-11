using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    // TODO: Review IWebElementExtensions class. Remove unused methods.
    public static class IWebElementExtensions
    {
        public static WebElementExtendedSearchContext Try(this IWebElement element)
        {
            return new WebElementExtendedSearchContext(element);
        }

        public static WebElementExtendedSearchContext Try(this IWebElement element, TimeSpan timeout)
        {
            return new WebElementExtendedSearchContext(element, timeout);
        }

        public static WebElementExtendedSearchContext Try(this IWebElement element, TimeSpan timeout, TimeSpan retryInterval)
        {
            return new WebElementExtendedSearchContext(element, timeout, retryInterval);
        }

        public static bool HasClass(this IWebElement element, string className)
        {
            return element.GetAttribute("class").Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Contains(className);
        }

        public static string GetValue(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        public static IWebElement FillInWith(this IWebElement element, string text)
        {
            element.Clear();
            if (!string.IsNullOrEmpty(text))
                element.SendKeys(text);
            return element;
        }

        public static string ToDetailedString(this IWebElement element)
        {
            element.CheckNotNull(nameof(element));

            try
            {
                return $@"Tag: {element.TagName}
Location: {element.Location}
Size: {element.Size}
Text: {element.Text.Trim()}";
            }
            catch
            {
                return null;
            }
        }
    }
}
