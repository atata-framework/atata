using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the screenshot information.
    /// Is used by the classes that implement <see cref="IScreenshotConsumer"/>.
    /// </summary>
    public class ScreenshotInfo
    {
        private readonly Lazy<Screenshot> _lazyScreenshot;

        public ScreenshotInfo() =>
            _lazyScreenshot = new Lazy<Screenshot>(CreateScreenshot);

        /// <summary>
        /// Gets the screenshot content with file extension.
        /// </summary>
        public FileContentWithExtension ScreenshotContent { get; internal set; }

        /// <summary>
        /// Gets the screenshot.
        /// </summary>
        public Screenshot Screenshot => _lazyScreenshot.Value;

        /// <summary>
        /// Gets the number.
        /// </summary>
        public int Number { get; internal set; }

        /// <summary>
        /// Gets the title. Can be <see langword="null"/>.
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// Gets the name of the page object that was shot.
        /// </summary>
        public string PageObjectName { get; internal set; }

        /// <summary>
        /// Gets the type name of the page object that was shot.
        /// </summary>
        public string PageObjectTypeName { get; internal set; }

        /// <summary>
        /// Gets the full name of the page object that was shot.
        /// </summary>
        public string PageObjectFullName { get; internal set; }

        private Screenshot CreateScreenshot()
        {
            string base64String = ScreenshotContent.ToBase64String();
            return new Screenshot(base64String);
        }
    }
}
