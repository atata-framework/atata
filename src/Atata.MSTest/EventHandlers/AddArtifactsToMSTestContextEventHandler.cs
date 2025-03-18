namespace Atata.MSTest;

public class AddArtifactsToMSTestContextEventHandler : ProcessFilesOnAtataContextDeInitCompletedEventHandlerBase
{
    private readonly TestContext _testContext;

    public AddArtifactsToMSTestContextEventHandler(TestContext testContext)
    {
        if (testContext is null)
            throw new ArgumentNullException(nameof(testContext));

        _testContext = testContext;
    }

    protected override string GetDirectoryPath(AtataContext context) =>
        context.ArtifactsPath;

    protected override void Process(FileInfo file) =>
        _testContext.AddResultFile(file.FullName);
}
