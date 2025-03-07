namespace Atata;

/// <summary>
/// Provides a screenshot taking methods.
/// </summary>
public interface IScreenshotTaker
{
    /// <summary>
    /// Takes a screenshot of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a screenshot.</param>
    void TakeScreenshot(string title = null);

    /// <summary>
    /// Takes a screenshot of the current page of a certain kind with an optionally specified title.
    /// </summary>
    /// <param name="kind">The kind of a screenshot.</param>
    /// <param name="title">The title of a screenshot.</param>
    void TakeScreenshot(ScreenshotKind kind, string title = null);
}
