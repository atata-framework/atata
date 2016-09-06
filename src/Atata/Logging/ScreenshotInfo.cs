using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the screenshot information. Is used by the classes that implement <see cref="IScreenshotConsumer"/>.
    /// </summary>
    public class ScreenshotInfo
    {
        /// <summary>
        /// Gets the screenshot.
        /// </summary>
        public Screenshot Screenshot { get; internal set; }

        /// <summary>
        /// Gets the number.
        /// </summary>
        public int Number { get; internal set; }

        /// <summary>
        /// Gets the title. Can be null.
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// Gets the name of the page object that was shot.
        /// </summary>
        public string PageObjectName { get; internal set; }

        /// <summary>
        /// Gets the full name of the page object that was shot.
        /// </summary>
        public string PageObjectFullName { get; internal set; }
    }
}
