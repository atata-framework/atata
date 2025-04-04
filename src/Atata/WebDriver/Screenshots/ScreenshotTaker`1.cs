namespace Atata;

internal sealed class ScreenshotTaker<TSession> : IScreenshotTaker
    where TSession : WebSession
{
    private readonly IScreenshotStrategy<TSession> _defaultScreenshotStrategy;

    private readonly IScreenshotStrategy<TSession> _viewportScreenshotStrategy;

    private readonly IScreenshotStrategy<TSession> _fullPageScreenshotStrategy;

    private readonly string _filePathTemplate;

    private readonly TSession _session;

    private int _screenshotNumber;

    public ScreenshotTaker(
        IScreenshotStrategy<TSession> defaultScreenshotStrategy,
        IScreenshotStrategy<TSession> viewportScreenshotStrategy,
        IScreenshotStrategy<TSession> fullPageScreenshotStrategy,
        string filePathTemplate,
        TSession session)
    {
        _defaultScreenshotStrategy = defaultScreenshotStrategy;
        _viewportScreenshotStrategy = viewportScreenshotStrategy;
        _fullPageScreenshotStrategy = fullPageScreenshotStrategy;
        _filePathTemplate = filePathTemplate;
        _session = session;
    }

    public void TakeScreenshot(string? title = null)
    {
        if (_defaultScreenshotStrategy is not null)
            TakeScreenshot(_defaultScreenshotStrategy, title);
    }

    public void TakeScreenshot(ScreenshotKind kind, string? title = null)
    {
        if (kind == ScreenshotKind.Viewport)
            TakeScreenshot(_viewportScreenshotStrategy, title);
        else if (kind == ScreenshotKind.FullPage)
            TakeScreenshot(_fullPageScreenshotStrategy, title);
        else
            TakeScreenshot(title);
    }

    private void TakeScreenshot(IScreenshotStrategy<TSession> strategy, string? title = null)
    {
        if (strategy is null || !_session.IsActive)
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

    private string FormatFilePath(string? title)
    {
        var pageObject = _session.PageObject;

        KeyValuePair<string, object?>[] snapshotVariables =
        [
            new("screenshot-number", _screenshotNumber),
            new("screenshot-title", title),
            new("screenshot-pageobjectname", pageObject?.ComponentName),
            new("screenshot-pageobjecttypename", pageObject?.ComponentTypeName),
            new("screenshot-pageobjectfullname", pageObject?.ComponentFullName)
        ];

        return _session.Variables.FillPathTemplateString(_filePathTemplate, snapshotVariables);
    }
}
