namespace Atata;

/// <summary>
/// Specifies the kind of screenshot.
/// </summary>
public enum ScreenshotKind
{
    /// <summary>
    /// The default, which is defined in configuration.
    /// </summary>
    Default,

    /// <summary>
    /// A screenshot of the viewport.
    /// </summary>
    Viewport,

    /// <summary>
    /// A screenshot of the full page.
    /// </summary>
    FullPage
}
