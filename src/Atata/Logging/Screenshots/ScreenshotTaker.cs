namespace Atata;

internal sealed class ScreenshotTaker
{
    private readonly IScreenshotStrategy _screenshotStrategy;

    private readonly string _filePathTemplate;

    private readonly WebDriverSession _session;

    private int _screenshotNumber;

    public ScreenshotTaker(
        IScreenshotStrategy screenshotStrategy,
        string filePathTemplate,
        WebDriverSession session)
    {
        _screenshotStrategy = screenshotStrategy;
        _filePathTemplate = filePathTemplate;
        _session = session;
    }

    public void TakeScreenshot(string title = null)
    {
        if (_screenshotStrategy is not null)
            TakeScreenshot(_screenshotStrategy, title);
    }

    public void TakeScreenshot(ScreenshotKind kind, string title = null)
    {
        if (kind == ScreenshotKind.Viewport)
            TakeScreenshot(WebDriverViewportScreenshotStrategy.Instance, title);
        else if (kind == ScreenshotKind.FullPage)
            TakeScreenshot(FullPageOrViewportScreenshotStrategy.Instance, title);
        else
            TakeScreenshot(title);
    }

    private void TakeScreenshot(IScreenshotStrategy strategy, string title = null)
    {
        if (strategy is null || !_session.HasDriver)
            return;

        _screenshotNumber++;

        try
        {
            _session.Log.ExecuteSection(
                new TakeScreenshotLogSection(_screenshotNumber, title),
                () =>
                {
                    FileContentWithExtension fileContent = strategy.TakeScreenshot(_session);
                    string filePath = FormatFilePath(title);

                    _session.Context.AddArtifact(filePath, fileContent, ArtifactTypes.Screenshot);
                    return filePath + fileContent.Extension;
                });
        }
        catch (Exception exception)
        {
            _session.Log.Error(exception, "Screenshot failed.");
        }
    }

    private string FormatFilePath(string title)
    {
        var pageObject = _session.PageObject;

        KeyValuePair<string, object>[] snapshotVariables =
        [
            new("screenshot-number", _screenshotNumber),
            new("screenshot-title", title),
            new("screenshot-pageobjectname", pageObject?.ComponentName),
            new("screenshot-pageobjecttypename", pageObject?.ComponentTypeName),
            new("screenshot-pageobjectfullname", pageObject?.ComponentFullName)
        ];

        return _session.FillPathTemplateString(_filePathTemplate, snapshotVariables);
    }
}
