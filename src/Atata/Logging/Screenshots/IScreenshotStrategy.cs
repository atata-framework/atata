namespace Atata
{
    /// <summary>
    /// A strategy that takes a page screenshot.
    /// </summary>
    public interface IScreenshotStrategy
    {
        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="context">The <see cref="AtataContext"/> instance.</param>
        /// <returns>The screenshot file content with extension for further saving.</returns>
        FileContentWithExtension TakeScreenshot(AtataContext context);
    }
}
