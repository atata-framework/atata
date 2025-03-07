namespace Atata.NUnit;

public sealed class AddArtifactsToNUnitTestContextEventHandler : AddDirectoryFilesToNUnitTestContextEventHandler
{
    public AddArtifactsToNUnitTestContextEventHandler()
        : base(context => context.ArtifactsPath)
    {
    }
}
