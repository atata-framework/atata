using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the strategy that takes a page viewport screenshot using <see cref="ITakesScreenshot.GetScreenshot"/> method
    /// of <see cref="AtataContext.Driver"/> instance.
    /// </summary>
    public sealed class WebDriverViewportScreenshotStrategy : IScreenshotStrategy
    {
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static WebDriverViewportScreenshotStrategy Instance { get; } =
            new WebDriverViewportScreenshotStrategy();

        /// <inheritdoc/>
        public FileContentWithExtension TakeScreenshot(AtataContext context)
        {
            Screenshot screenshot = context.Driver.AsScreenshotTaker().GetScreenshot();
            return FileContentWithExtension.CreateFromByteArray(screenshot.AsByteArray, ".png");
        }
    }
}
