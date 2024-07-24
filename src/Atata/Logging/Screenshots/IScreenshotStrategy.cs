namespace Atata;

/// <summary>
/// A strategy that takes a page screenshot.
/// </summary>
public interface IScreenshotStrategy
{
    /// <summary>
    /// Takes the screenshot.
    /// </summary>
    /// <param name="session">The <see cref="WebDriverSession"/> instance.</param>
    /// <returns>The screenshot file content with extension for further saving.</returns>
    FileContentWithExtension TakeScreenshot(WebDriverSession session);
}
