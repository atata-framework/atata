namespace Atata;

/// <summary>
/// Represents an event that occurs when a screenshot file is saved.
/// </summary>
[Obsolete("Use ArtifactAddedEvent instead with checking its ArtifactType property is ArtifactTypes.Screenshot.")] // Obsolete since v2.11.0.
public class ScreenshotFileSavedEvent
{
    public ScreenshotFileSavedEvent(ScreenshotInfo screenshotInfo, string filePath)
    {
        ScreenshotInfo = screenshotInfo;
        FilePath = filePath;
    }

    /// <summary>
    /// Gets the screenshot information.
    /// </summary>
    public ScreenshotInfo ScreenshotInfo { get; }

    /// <summary>
    /// Gets the file path.
    /// </summary>
    public string FilePath { get; }
}
