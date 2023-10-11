namespace Atata;

[Obsolete("Use " + nameof(AddArtifactsToNUnitTestContextEventHandler) + " instead.")] // Obsolete since v2.11.0.
public class AddArtifactsToNUnitTestContextOnCleanUpEventHandler : AddDirectoryFilesToNUnitTestContextOnCleanUpEventHandler
{
    public AddArtifactsToNUnitTestContextOnCleanUpEventHandler()
        : base(context => context.Artifacts.FullName.Value)
    {
    }
}
