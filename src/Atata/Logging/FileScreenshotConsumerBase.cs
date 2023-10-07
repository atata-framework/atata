namespace Atata;

public abstract class FileScreenshotConsumerBase : IScreenshotConsumer
{
    /// <summary>
    /// Gets or sets the image format.
    /// The default format is <see cref="OpenQA.Selenium.ScreenshotImageFormat.Png"/>.
    /// </summary>
    public ScreenshotImageFormat ImageFormat { get; set; } = ScreenshotImageFormat.Png;

    /// <summary>
    /// Takes the specified screenshot.
    /// </summary>
    /// <param name="screenshotInfo">The screenshot information.</param>
    public void Take(ScreenshotInfo screenshotInfo)
    {
        string filePath = BuildFilePath(screenshotInfo);
        filePath = filePath.SanitizeForPath();
        filePath += ImageFormat.GetExtension();

        if (!Path.IsPathRooted(filePath))
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

        string directoryPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        SaveImage(screenshotInfo, filePath);

        var context = AtataContext.Current;
        context.Log.Info($"Screenshot saved to file \"{filePath}\"");

#pragma warning disable CS0618 // Type or member is obsolete
        context.EventBus.Publish(new ScreenshotFileSavedEvent(screenshotInfo, filePath));
#pragma warning restore CS0618 // Type or member is obsolete

        string artifactRelativeFilePath = filePath.StartsWith(context.Artifacts.FullName, StringComparison.OrdinalIgnoreCase)
            ? filePath.Substring(context.Artifacts.FullName.Value.Length)
            : filePath;

        context.EventBus.Publish(new ArtifactAddedEvent(filePath, artifactRelativeFilePath, ArtifactTypes.Screenshot, screenshotInfo.Title));
    }

    /// <summary>
    /// Builds the path of the file without the extension.
    /// </summary>
    /// <param name="screenshotInfo">The screenshot information.</param>
    /// <returns>The file path without the extension.</returns>
    protected abstract string BuildFilePath(ScreenshotInfo screenshotInfo);

    private void SaveImage(ScreenshotInfo screenshotInfo, string filePath)
    {
        if (ImageFormat == ScreenshotImageFormat.Png)
            screenshotInfo.ScreenshotContent.Save(filePath);
        else
            screenshotInfo.Screenshot.SaveAsFile(filePath, ImageFormat);
    }
}
