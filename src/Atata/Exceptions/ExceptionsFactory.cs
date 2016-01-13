using OpenQA.Selenium;
using System.Text;

namespace Atata
{
    internal static class ExceptionsFactory
    {
        public static NoSuchElementException CreateForNoSuchElement(string elementName = null, By by = null)
        {
            string message = CreateMessageForNoSuchElement(elementName, by);
            return new NoSuchElementException(message);
        }

        private static string CreateMessageForNoSuchElement(string elementName, By by)
        {
            StringBuilder builder = new StringBuilder("Unable to locate element");

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
