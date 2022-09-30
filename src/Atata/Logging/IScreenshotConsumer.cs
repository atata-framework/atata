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
        /// <param name="screenshotInfo">The screenshot information object.</param>
        void Take(ScreenshotInfo screenshotInfo);
    }
}
