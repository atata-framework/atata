using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="IWebElement"/>
    /// that wrap actual methods with log sections.
    /// </summary>
    public static class IWebElementLoggingExtensions
    {
        /// <summary>
        /// Clears the value of an element within a log section.
        /// </summary>
        /// <param name="element">The element.</param>
        public static void ClearWithLogging(this IWebElement element)
        {
            ILogManager log = AtataContext.Current?.Log;

            if (log != null)
            {
                log.ExecuteSection(
                    new ElementClearLogSection(element),
                    () => element.Clear());
            }
            else
            {
                element.Clear();
            }
        }

        /// <summary>
        /// Clicks an element within a log section.
        /// </summary>
        /// <param name="element">The element.</param>
        public static void ClickWithLogging(this IWebElement element)
        {
            ILogManager log = AtataContext.Current?.Log;

            if (log != null)
            {
                log.ExecuteSection(
                    new ElementClickLogSection(element),
                    () => element.Click());
            }
            else
            {
                element.Click();
            }
        }

        /// <summary>
        /// Sends the keys to an element within a log section.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="text">The text.</param>
        public static void SendKeysWithLogging(this IWebElement element, string text)
        {
            ILogManager log = AtataContext.Current?.Log;

            if (log != null)
            {
                log.ExecuteSection(
                    new ElementSendKeysLogSection(element, text),
                    () => element.SendKeys(text));
            }
            else
            {
                element.SendKeys(text);
            }
        }
    }
}
