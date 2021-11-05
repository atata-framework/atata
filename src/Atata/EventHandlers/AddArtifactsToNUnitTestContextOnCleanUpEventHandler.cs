namespace Atata
{
    public class AddArtifactsToNUnitTestContextOnCleanUpEventHandler : AddDirectoryFilesToNUnitTestContextOnCleanUpEventHandler
    {
        public AddArtifactsToNUnitTestContextOnCleanUpEventHandler()
            : base(context => context.Artifacts.Value.FullName)
        {
        }
    }
}
