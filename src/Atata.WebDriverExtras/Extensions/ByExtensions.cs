using System;
using OpenQA.Selenium;

namespace Atata
{
    public static class ByExtensions
    {
        public static By OfKind(this By by, string kind, string name = null)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.ElementKind = kind;
            return name != null ? extendedBy.Named(name) : extendedBy;
        }

        public static By Named(this By by, string name)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.ElementName = name;

            if (name != null && extendedBy.ToString().Contains("{0}"))
            {
                return extendedBy.FormatWith(name);
            }

            return extendedBy;
        }

        public static By Safely(this By by, bool isSafely = true)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.IsSafely = isSafely;
            return extendedBy;
        }

        public static By Hidden(this By by)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Visibility = Visibility.Hidden;
            return extendedBy;
        }

        public static By OfAnyVisibility(this By by)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Visibility = Visibility.Any;
            return extendedBy;
        }

        public static By Within(this By by, TimeSpan timeout)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Timeout = timeout;
            return extendedBy;
        }

        public static By Within(this By by, TimeSpan timeout, TimeSpan retryInterval)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Timeout = timeout;
            extendedBy.Options.RetryInterval = retryInterval;
            return extendedBy;
        }

        public static By AtOnce(this By by)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Timeout = TimeSpan.Zero;
            return extendedBy;
        }

        public static By SafelyAtOnce(this By by, bool isSafely = true)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.IsSafely = isSafely;
            extendedBy.Options.Timeout = TimeSpan.Zero;
            return extendedBy;
        }

        public static By With(this By by, SearchOptions options)
        {
            options = options ?? new SearchOptions();

            ExtendedBy extendedBy = new ExtendedBy(by);

            if (options.IsTimeoutSet)
                extendedBy.Options.Timeout = options.Timeout;

            if (options.IsRetryIntervalSet)
                extendedBy.Options.RetryInterval = options.RetryInterval;

            if (options.IsVisibilitySet)
                extendedBy.Options.Visibility = options.Visibility;

            if (options.IsSafelySet)
                extendedBy.Options.IsSafely = options.IsSafely;

            return extendedBy;
        }

        public static By FormatWith(this By by, params object[] args)
        {
            string selector = string.Format(by.GetSelector(), args);
            By formattedBy = CreateBy(by.GetMethod(), selector);

            ExtendedBy extendedBy = new ExtendedBy(formattedBy);

            ExtendedBy originalByAsExtended = by as ExtendedBy;
            if (originalByAsExtended != null)
            {
                extendedBy.ElementName = originalByAsExtended.ElementName;
                extendedBy.ElementKind = originalByAsExtended.ElementKind;
                extendedBy.Options = originalByAsExtended.Options;
            }

            return extendedBy;
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
