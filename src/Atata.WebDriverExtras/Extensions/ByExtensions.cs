using System;
using OpenQA.Selenium;

namespace Atata
{
    public static class ByExtensions
    {
        public static By OfKind(this By by, string kind, string name = null)
        {
            ByOptions options = ByOptionsMap.GetAndStore(by);
            options.Kind = kind;
            return name != null ? by.Named(name) : by;
        }

        public static By Named(this By by, string name)
        {
            ByOptions options = ByOptionsMap.GetAndStore(by);
            options.Name = name;

            if (name != null && by.ToString().Contains("{0}"))
            {
                By newBy = by.FormatWith(name);
                ByOptionsMap.Replace(by, newBy);

                return newBy;
            }

            return by;
        }

        public static By Safely(this By by, bool safely = true)
        {
            ByOptionsMap.GetAndStore(by).ThrowOnFail = !safely;
            return by;
        }

        public static By Invisible(this By by)
        {
            ByOptionsMap.GetAndStore(by).Visibility = ElementVisibility.Invisible;
            return by;
        }

        public static By OfAnyVisibility(this By by)
        {
            ByOptionsMap.GetAndStore(by).Visibility = ElementVisibility.Any;
            return by;
        }

        public static By WithRetry(this By by, TimeSpan timeout)
        {
            ByOptionsMap.GetAndStore(by).Timeout = timeout;
            return by;
        }

        public static By WithRetry(this By by, TimeSpan timeout, TimeSpan retryInterval)
        {
            ByOptions options = ByOptionsMap.GetAndStore(by);
            options.Timeout = timeout;
            options.RetryInterval = retryInterval;
            return by;
        }

        public static By Immediately(this By by)
        {
            ByOptionsMap.GetAndStore(by).Timeout = TimeSpan.Zero;
            return by;
        }

        public static By SafelyAndImmediately(this By by, bool isSafely = true)
        {
            ByOptions options = ByOptionsMap.GetAndStore(by);
            options.ThrowOnFail = !isSafely;
            options.Timeout = TimeSpan.Zero;
            return by;
        }

        public static By With(this By by, SearchOptions options)
        {
            if (options == null)
                return by;

            ByOptions byOptions = ByOptionsMap.GetAndStore(by);

            byOptions.Timeout = options.Timeout;
            byOptions.RetryInterval = options.RetryInterval;
            byOptions.Visibility = options.Visibility;
            byOptions.ThrowOnFail = !options.IsSafely;

            return by;
        }

        public static By FormatWith(this By by, params object[] args)
        {
            string selector = string.Format(by.GetSelector(), args);
            return CreateBy(by.GetMethod(), selector);
        }

        public static string GetMethod(this By by)
        {
            return by.ToString().Split(':')[0].Replace("By.", string.Empty);
        }

        public static string GetSelector(this By by)
        {
            string text = by.ToString();
            return text.Substring(text.IndexOf(':') + 2);
        }

        private static By CreateBy(string method, string selector)
        {
            switch (method)
            {
                case "Id":
                    return By.Id(selector);
                case "LinkText":
                    return By.LinkText(selector);
                case "Name":
                    return By.Name(selector);
                case "XPath":
                    return By.XPath(selector);
                case "ClassName[Contains]":
                    return By.ClassName(selector);
                case "PartialLinkText":
                    return By.PartialLinkText(selector);
                case "TagName":
                    return By.TagName(selector);
                case "CssSelector":
                    return By.CssSelector(selector);
                default:
                    throw new ArgumentException(string.Format("Unknown '{0}' method of OpenQA.Selenium.By", method), "method");
            }
        }
    }
}
