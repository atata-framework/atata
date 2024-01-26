namespace Atata;

internal sealed class PageSnapshotTaker
{
    private readonly IPageSnapshotStrategy _snapshotStrategy;

    private readonly string _filePathTemplate;

    private readonly AtataContext _context;

    private int _snapshotNumber;

    public PageSnapshotTaker(
        IPageSnapshotStrategy snapshotStrategy,
        string filePathTemplate,
        AtataContext context)
    {
        _snapshotStrategy = snapshotStrategy;
        _filePathTemplate = filePathTemplate;
        _context = context;
    }

    public void TakeSnapshot(string title = null)
    {
        if (_snapshotStrategy is null || !_context.HasDriver)
            return;

        _snapshotNumber++;

        try
        {
            _context.Log.ExecuteSection(
                new TakePageSnapshotLogSection(_snapshotNumber, title),
                () =>
                {
                    FileContentWithExtension fileContent = _snapshotStrategy.TakeSnapshot(_context);
                    string filePath = FormatFilePath(title);

                    _context.AddArtifact(filePath, fileContent, ArtifactTypes.PageSnapshot);
                    return filePath + fileContent.Extension;
                });
        }
        catch (Exception exception)
        {
            _context.Log.Error(exception, "Page snapshot failed.");
        }
    }

    private string FormatFilePath(string title)
    {
        var pageObject = _context.PageObject;

        KeyValuePair<string, object>[] snapshotVariables =
        [
            new("snapshot-number", _snapshotNumber),
            new("snapshot-title", title),
            new("snapshot-pageobjectname", pageObject?.ComponentName),
            new("snapshot-pageobjecttypename", pageObject?.ComponentTypeName),
            new("snapshot-pageobjectfullname", pageObject?.ComponentFullName)
        ];

        return _context.FillPathTemplateString(_filePathTemplate, snapshotVariables);
    }
}
