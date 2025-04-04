namespace Atata;

public abstract class ProcessFilesOnAtataContextDeInitCompletedEventHandlerBase : IEventHandler<AtataContextDeInitCompletedEvent>
{
    public void Handle(AtataContextDeInitCompletedEvent eventData, AtataContext context)
    {
        string directoryPath = GetDirectoryPath(context);

        DirectoryInfo directory = new(directoryPath);

        if (directory.Exists)
        {
            var files = directory.EnumerateFiles("*", SearchOption.AllDirectories)
                .OrderBy(x => x.CreationTimeUtc);

            foreach (var file in files)
                Process(file);
        }
    }

    protected abstract string GetDirectoryPath(AtataContext context);

    protected abstract void Process(FileInfo file);
}
