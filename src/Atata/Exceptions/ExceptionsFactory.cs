using Humanizer;
using OpenQA.Selenium;
using System;
using System.Text;

namespace Atata
{
    internal static class ExceptionsFactory
    {
        internal static NoSuchElementException CreateForNoSuchElement(string elementName = null, By by = null)
        {
            string message = BuildElementErrorMessage("Unable to locate element", elementName, by);
            return new NoSuchElementException(message);
        }

        internal static NotMissingElementException CreateForNotMissingElement(string elementName = null, By by = null)
        {
            string message = BuildElementErrorMessage("Able to locate element that should be missing", elementName, by);
            return new NotMissingElementException(message);
        }

        internal static ArgumentException CreateForUnsuppotedEnumValue<T>(T value, string paramName)
            where T : struct
        {
            string message = "Unsopported {0} value: {1}.".FormatWith(typeof(T).FullName, value);
            return new ArgumentException(message, paramName);
        }

        private static string BuildElementErrorMessage(string message, string elementName, By by)
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

            return builder.ToString();
        }
    }
}
