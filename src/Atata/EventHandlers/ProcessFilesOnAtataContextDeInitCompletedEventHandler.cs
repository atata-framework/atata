namespace Atata;

public class ProcessFilesOnAtataContextDeInitCompletedEventHandler : ProcessFilesOnAtataContextDeInitCompletedEventHandlerBase
{
    private readonly Func<AtataContext, string> _directoryPathBuilder;

    private readonly Action<FileInfo> _fileProcessor;

    public ProcessFilesOnAtataContextDeInitCompletedEventHandler(
        string directoryPath,
        Action<FileInfo> fileProcessor)
    {
        Guard.ThrowIfNullOrWhitespace(directoryPath);
        Guard.ThrowIfNull(fileProcessor);

        _directoryPathBuilder = _ => directoryPath;
        _fileProcessor = fileProcessor;
    }

    public ProcessFilesOnAtataContextDeInitCompletedEventHandler(
        Func<AtataContext, string> directoryPathBuilder,
        Action<FileInfo> fileProcessor)
    {
        Guard.ThrowIfNull(directoryPathBuilder);
        Guard.ThrowIfNull(fileProcessor);

        _directoryPathBuilder = directoryPathBuilder;
        _fileProcessor = fileProcessor;
    }

    protected override string GetDirectoryPath(AtataContext context) =>
        _directoryPathBuilder.Invoke(context);

    protected override void Process(FileInfo file) =>
        _fileProcessor.Invoke(file);
}
