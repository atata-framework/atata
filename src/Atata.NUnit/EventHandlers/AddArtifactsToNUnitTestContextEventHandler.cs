namespace Atata.NUnit;

public sealed class AddArtifactsToNUnitTestContextEventHandler : ProcessFilesOnAtataContextDeInitCompletedEventHandlerBase
{
    protected override string GetDirectoryPath(AtataContext context) =>
        context.ArtifactsPath;

    protected override void Process(FileInfo file) =>
        TestContext.AddTestAttachment(file.FullName);
}
