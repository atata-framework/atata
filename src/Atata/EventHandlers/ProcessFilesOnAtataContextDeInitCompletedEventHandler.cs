#nullable enable

namespace Atata;

public class ProcessFilesOnAtataContextDeInitCompletedEventHandler : ProcessFilesOnAtataContextDeInitCompletedEventHandlerBase
{
    private readonly Func<AtataContext, string> _directoryPathBuilder;

    private readonly Action<FileInfo> _fileProcessor;

    public ProcessFilesOnAtataContextDeInitCompletedEventHandler(
        string directoryPath,
        Action<FileInfo> fileProcessor)
    {
        directoryPath.CheckNotNullOrWhitespace(nameof(directoryPath));
        _directoryPathBuilder = _ => directoryPath;
        _fileProcessor = fileProcessor.CheckNotNull(nameof(fileProcessor));
    }

    public ProcessFilesOnAtataContextDeInitCompletedEventHandler(
        Func<AtataContext, string> directoryPathBuilder,
        Action<FileInfo> fileProcessor)
    {
        _directoryPathBuilder = directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));
        _fileProcessor = fileProcessor.CheckNotNull(nameof(fileProcessor));
    }

    protected override string GetDirectoryPath(AtataContext context) =>
        _directoryPathBuilder.Invoke(context);

    protected override void Process(FileInfo file) =>
        _fileProcessor.Invoke(file);
}
