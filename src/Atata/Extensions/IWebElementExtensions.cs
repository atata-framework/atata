using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

        public static bool HasContent(this IWebElement element, string content)
        {
            return element.Text.Contains(content);
        }

        public static IWebElement NullIfInvisible(this IWebElement element)
        {
            return element.Displayed ? element : null;
        }

        public static ReadOnlyCollection<IWebElement> OnlyVisible(this ReadOnlyCollection<IWebElement> source)
        {
            return source.Where(x => x.Displayed).ToReadOnly();
        }

        public static IWebElement FirstVisibleOrDefault(this ReadOnlyCollection<IWebElement> source)
        {
            return source.Where(x => x.Displayed).FirstOrDefault();
        }

        public static string GetValue(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        public static IWebElement FillInWith(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
            return element;
        }
    }
}
