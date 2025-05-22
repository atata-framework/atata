namespace Atata;

internal sealed class ScreenshotTaker<TSession> : IScreenshotTaker, IResetsCounter
    where TSession : WebSession
{
    private readonly IScreenshotStrategy<TSession> _defaultScreenshotStrategy;

    private readonly IScreenshotStrategy<TSession> _viewportScreenshotStrategy;

    private readonly IScreenshotStrategy<TSession> _fullPageScreenshotStrategy;

    private readonly string _filePathTemplate;

    private readonly bool _prependArtifactNumberToFileName;

    private readonly TSession _session;

    private int _screenshotNumber;

    public ScreenshotTaker(
        IScreenshotStrategy<TSession> defaultScreenshotStrategy,
        IScreenshotStrategy<TSession> viewportScreenshotStrategy,
        IScreenshotStrategy<TSession> fullPageScreenshotStrategy,
        string filePathTemplate,
        bool prependArtifactNumberToFileName,
        TSession session)
    {
        _defaultScreenshotStrategy = defaultScreenshotStrategy;
        _viewportScreenshotStrategy = viewportScreenshotStrategy;
        _fullPageScreenshotStrategy = fullPageScreenshotStrategy;
        _filePathTemplate = filePathTemplate;
        _prependArtifactNumberToFileName = prependArtifactNumberToFileName;
        _session = session;
    }

    public FileSubject? TakeScreenshot(string? title = null) =>
        TakeScreenshot(_defaultScreenshotStrategy, title);

    public FileSubject? TakeScreenshot(ScreenshotKind kind, string? title = null) =>
        kind switch
        {
            ScreenshotKind.Viewport => TakeScreenshot(_viewportScreenshotStrategy, title),
            ScreenshotKind.FullPage => TakeScreenshot(_fullPageScreenshotStrategy, title),
            _ => TakeScreenshot(title)
        };

    public void ResetCounter() =>
        Interlocked.Exchange(ref _screenshotNumber, 0);

    private FileSubject? TakeScreenshot(IScreenshotStrategy<TSession> strategy, string? title = null)
    {
        if (strategy is null || !_session.IsActive)
            return null;

        Interlocked.Increment(ref _screenshotNumber);

        try
        {
            return _session.Log.ExecuteSection(
                new TakeScreenshotLogSection(_screenshotNumber, title),
                () =>
                {
                    FileContentWithExtension fileContent = strategy.TakeScreenshot(_session);

                    string filePath = FormatFilePath(title);
                    filePath = WebDriverArtifactFileUtils.SanitizeFileName(filePath);

                    return _session.Context.AddArtifact(
                        filePath,
                        fileContent,
                        new()
                        {
                            ArtifactType = ArtifactTypes.Screenshot,
                            ArtifactTitle = title,
                            PrependArtifactNumberToFileName = _prependArtifactNumberToFileName
                        });
                });
        }
        catch (Exception exception)
        {
            _session.Log.Error(exception, "Screenshot failed.");
            return null;
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
