namespace Atata.NUnit;

public class AddDirectoryFilesToNUnitTestContextEventHandler : IEventHandler<AtataContextDeInitCompletedEvent>
{
    private readonly Func<AtataContext, string> _directoryPathBuilder;

    public AddDirectoryFilesToNUnitTestContextEventHandler(string directoryPath)
    {
        directoryPath.CheckNotNullOrWhitespace(nameof(directoryPath));
        _directoryPathBuilder = _ => directoryPath;
    }

    public AddDirectoryFilesToNUnitTestContextEventHandler(Func<AtataContext, string> directoryPathBuilder) =>
        _directoryPathBuilder = directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));

    public void Handle(AtataContextDeInitCompletedEvent eventData, AtataContext context)
    {
        string directoryPath = _directoryPathBuilder.Invoke(context);

        directoryPath = context.Variables.FillPathTemplateString(directoryPath);

        DirectoryInfo directory = new DirectoryInfo(directoryPath);

        if (directory.Exists)
        {
            var files = directory.EnumerateFiles("*", SearchOption.AllDirectories)
                .OrderBy(x => x.CreationTimeUtc);

            foreach (var file in files)
                TestContext.AddTestAttachment(file.FullName);
        }
    }
}
