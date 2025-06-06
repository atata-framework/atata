﻿namespace Atata;

internal sealed class PageSnapshotTaker<TSession> : IPageSnapshotTaker, IResetsCounter
    where TSession : WebSession
{
    private readonly IPageSnapshotStrategy<TSession> _snapshotStrategy;

    private readonly string _filePathTemplate;

    private readonly bool _prependArtifactNumberToFileName;

    private readonly TSession _session;

    private int _snapshotNumber;

    public PageSnapshotTaker(
        IPageSnapshotStrategy<TSession> snapshotStrategy,
        string filePathTemplate,
        bool prependArtifactNumberToFileName,
        TSession session)
    {
        _snapshotStrategy = snapshotStrategy;
        _filePathTemplate = filePathTemplate;
        _prependArtifactNumberToFileName = prependArtifactNumberToFileName;
        _session = session;
    }

    public FileSubject? TakeSnapshot(string? title = null)
    {
        if (_snapshotStrategy is null || !_session.IsActive)
            return null;

        Interlocked.Increment(ref _snapshotNumber);

        try
        {
            return _session.Log.ExecuteSection(
                new TakePageSnapshotLogSection(_snapshotNumber, title),
                () =>
                {
                    FileContentWithExtension fileContent = _snapshotStrategy.TakeSnapshot(_session);

                    string filePath = FormatFilePath(title);
                    filePath = WebDriverArtifactFileUtils.SanitizeFileName(filePath);

                    return _session.Context.AddArtifact(
                        filePath,
                        fileContent,
                        new()
                        {
                            ArtifactType = ArtifactTypes.PageSnapshot,
                            ArtifactTitle = title,
                            PrependArtifactNumberToFileName = _prependArtifactNumberToFileName
                        });
                });
        }
        catch (Exception exception)
        {
            _session.Log.Error(exception, "Page snapshot failed.");
            return null;
        }
    }

    public void ResetCounter() =>
        Interlocked.Exchange(ref _snapshotNumber, 0);

    private string FormatFilePath(string? title)
    {
        var pageObject = _session.PageObject;

        KeyValuePair<string, object?>[] snapshotVariables =
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
