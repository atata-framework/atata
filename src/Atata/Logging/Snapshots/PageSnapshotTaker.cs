namespace Atata;

internal sealed class PageSnapshotTaker
{
    private readonly IPageSnapshotStrategy _snapshotStrategy;

    private readonly string _filePathTemplate;

    private readonly WebDriverSession _session;

    private int _snapshotNumber;

    public PageSnapshotTaker(
        IPageSnapshotStrategy snapshotStrategy,
        string filePathTemplate,
        WebDriverSession session)
    {
        _snapshotStrategy = snapshotStrategy;
        _filePathTemplate = filePathTemplate;
        _session = session;
    }

    public void TakeSnapshot(string title = null)
    {
        if (_snapshotStrategy is null || !_session.HasDriver)
            return;

        _snapshotNumber++;

        try
        {
            _session.Log.ExecuteSection(
                new TakePageSnapshotLogSection(_snapshotNumber, title),
                () =>
                {
                    FileContentWithExtension fileContent = _snapshotStrategy.TakeSnapshot(_session);
                    string filePath = FormatFilePath(title);

                    _session.Context.AddArtifact(filePath, fileContent, ArtifactTypes.PageSnapshot);
                    return filePath + fileContent.Extension;
                });
        }
        catch (Exception exception)
        {
            _session.Log.Error(exception, "Page snapshot failed.");
        }
    }

    private string FormatFilePath(string title)
    {
        var pageObject = _session.PageObject;

        KeyValuePair<string, object>[] snapshotVariables =
        [
            new("snapshot-number", _snapshotNumber),
            new("snapshot-title", title),
            new("snapshot-pageobjectname", pageObject?.ComponentName),
            new("snapshot-pageobjecttypename", pageObject?.ComponentTypeName),
            new("snapshot-pageobjectfullname", pageObject?.ComponentFullName)
        ];

        return _session.Variables.FillPathTemplateString(_filePathTemplate, snapshotVariables);
    }
}
