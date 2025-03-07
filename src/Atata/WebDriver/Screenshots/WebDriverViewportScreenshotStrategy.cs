namespace Atata;

/// <summary>
/// Represents a <see cref="WebDriverSession"/> screenshot strategy that takes a page viewport screenshot
/// using <see cref="ITakesScreenshot.GetScreenshot"/> method of <see cref="WebDriverSession.Driver"/> instance.
/// </summary>
public sealed class WebDriverViewportScreenshotStrategy : IScreenshotStrategy<WebDriverSession>
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static WebDriverViewportScreenshotStrategy Instance { get; } =
        new WebDriverViewportScreenshotStrategy();

    /// <inheritdoc/>
    public FileContentWithExtension TakeScreenshot(WebDriverSession session)
    {
        Screenshot screenshot = session.Driver.AsScreenshotTaker().GetScreenshot();
        return FileContentWithExtension.CreateFromByteArray(screenshot.AsByteArray, ".png");
    }
}
