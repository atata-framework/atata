using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Defines the interface for the screenshot consumer.
    /// </summary>
    public interface IScreenshotConsumer
    {
        /// <summary>
        /// Takes the specified screenshot.
        /// </summary>
        /// <param name="screenshot">The screenshot.</param>
        /// <param name="number">The number of the screenshot.</param>
        /// <param name="title">The title. Can be null.</param>
        void Take(Screenshot screenshot, int number, string title);
    }
}
