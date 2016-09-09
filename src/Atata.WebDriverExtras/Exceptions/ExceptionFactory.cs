using System;
using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    public static class ExceptionFactory
    {
        public static NoSuchElementException CreateForNoSuchElement(string elementName = null, By by = null, ISearchContext searchContext = null)
        {
            elementName = elementName ?? (by as ExtendedBy)?.GetElementNameWithKind();

            string message = BuildElementErrorMessage("Unable to locate element", elementName, by, searchContext);
            return new NoSuchElementException(message);
        }

        public static NotMissingElementException CreateForNotMissingElement(string elementName = null, By by = null, ISearchContext searchContext = null)
        {
            elementName = elementName ?? (by as ExtendedBy)?.GetElementNameWithKind();

            string message = BuildElementErrorMessage("Able to locate element that should be missing", elementName, by, searchContext);
            return new NotMissingElementException(message);
        }

        public static ArgumentException CreateForUnsupportedEnumValue<T>(T value, string paramName)
            where T : struct
        {
            string message = string.Format("Unsupported {0} enum value: {1}.", typeof(T).FullName, value);
            return new ArgumentException(message, paramName);
        }

        public static string BuildElementErrorMessage(string message, string elementName, By by, ISearchContext searchContext = null)
        {
            StringBuilder builder = new StringBuilder(message);

            bool hasName = !string.IsNullOrWhiteSpace(elementName);
            bool hasBy = by != null;

            if (hasName || hasBy)
            {
                builder.Append(": ");

                if (hasName && hasBy)
                    builder.AppendFormat("{0}. {1}", elementName, by);
                else if (!string.IsNullOrWhiteSpace(elementName))
                    builder.Append(elementName);
                else if (by != null)
                    builder.Append(by);
            }

            string searchContextString = SearchContextToString(searchContext);
            if (searchContextString != null)
                builder.AppendLine().Append(searchContextString);

            return builder.ToString();
        }

        private static string SearchContextToString(ISearchContext context)
        {
            IWebElement element = context as IWebElement;
            if (element != null)
            {
                return $@"Context element:
{element.ToDetailedString()}";
            }
            else
            {
                return null;
            }
        }
    }
}
