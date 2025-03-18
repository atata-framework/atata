namespace Atata.NUnit;

public class AddDirectoryFilesToNUnitTestContextEventHandler : ProcessFilesOnAtataContextDeInitCompletedEventHandlerBase
{
    private readonly Func<AtataContext, string> _directoryPathBuilder;

    public AddDirectoryFilesToNUnitTestContextEventHandler(string directoryPath)
    {
        directoryPath.CheckNotNullOrWhitespace(nameof(directoryPath));
        _directoryPathBuilder = _ => directoryPath;
    }

    public AddDirectoryFilesToNUnitTestContextEventHandler(Func<AtataContext, string> directoryPathBuilder) =>
        _directoryPathBuilder = directoryPathBuilder.CheckNotNull(nameof(directoryPathBuilder));

    protected override string GetDirectoryPath(AtataContext context)
    {
        string directoryPath = _directoryPathBuilder.Invoke(context);
        return context.Variables.FillPathTemplateString(directoryPath);
    }

    protected override void Process(FileInfo file) =>
        TestContext.AddTestAttachment(file.FullName);
}
