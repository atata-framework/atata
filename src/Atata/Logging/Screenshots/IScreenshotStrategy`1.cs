namespace Atata;

/// <summary>
/// A strategy that takes a page screenshot.
/// </summary>
/// <typeparam name="TSession">The type of the session.</typeparam>
public interface IScreenshotStrategy<in TSession>
    where TSession : AtataSession
{
    /// <summary>
    /// Takes a screenshot.
    /// </summary>
    /// <param name="session">The <typeparamref name="TSession"/> instance.</param>
    /// <returns>The screenshot file content with extension for further saving.</returns>
    FileContentWithExtension TakeScreenshot(TSession session);
}
