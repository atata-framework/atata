namespace Atata;

public sealed class AddArtifactsToNUnitTestContextEventHandler : AddDirectoryFilesToNUnitTestContextEventHandler
{
    public AddArtifactsToNUnitTestContextEventHandler()
        : base(context => context.Artifacts.FullName.Value)
    {
    }
}
