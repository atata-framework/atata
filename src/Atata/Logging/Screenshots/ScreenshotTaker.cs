namespace Atata;

internal sealed class ScreenshotTaker
{
    private readonly IScreenshotStrategy _screenshotStrategy;

    private readonly string _filePathTemplate;

    private readonly AtataContext _context;

    private int _screenshotNumber;

    public ScreenshotTaker(
        IScreenshotStrategy screenshotStrategy,
        string filePathTemplate,
        AtataContext context)
    {
        _screenshotStrategy = screenshotStrategy;
        _filePathTemplate = filePathTemplate;
        _context = context;
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
        if (strategy is null || !_context.HasDriver)
            return;

        _screenshotNumber++;

        try
        {
            _context.Log.ExecuteSection(
                new TakeScreenshotLogSection(_screenshotNumber, title),
                () =>
                {
                    FileContentWithExtension fileContent = strategy.TakeScreenshot(_context);
                    string filePath = FormatFilePath(title);

                    _context.AddArtifact(filePath, fileContent, ArtifactTypes.Screenshot);
                    return filePath + fileContent.Extension;
                });
        }
        catch (Exception exception)
        {
            _context.Log.Error(exception, "Screenshot failed.");
        }
    }

    private string FormatFilePath(string title)
    {
        var pageObject = _context.PageObject;

        KeyValuePair<string, object>[] snapshotVariables =
        [
            new("screenshot-number", _screenshotNumber),
            new("screenshot-title", title),
            new("screenshot-pageobjectname", pageObject?.ComponentName),
            new("screenshot-pageobjecttypename", pageObject?.ComponentTypeName),
            new("screenshot-pageobjectfullname", pageObject?.ComponentFullName)
        ];

        return _context.FillPathTemplateString(_filePathTemplate, snapshotVariables);
    }
}
