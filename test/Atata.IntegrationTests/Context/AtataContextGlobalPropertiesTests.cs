namespace Atata.IntegrationTests.Context;

public static class AtataContextGlobalPropertiesTests
{
    [Parallelizable(ParallelScope.None)]
    public class UseArtifactsRootPathTemplate : SessionlessTestSuite
    {
        private readonly string _rootPath = $"{AppDomain.CurrentDomain.BaseDirectory}artifacts{Path.DirectorySeparatorChar}{Guid.NewGuid()}";

        [OneTimeSetUp]
        public void SetUpFixture() =>
            AtataContext.GlobalProperties.UseArtifactsRootPathTemplate(_rootPath);

        [OneTimeTearDown]
        public void TearDownFixture() =>
            AtataContext.GlobalProperties.UseArtifactsRootPathTemplate(AtataContextGlobalProperties.DefaultArtifactsRootPathTemplate);

        [Test]
        public void AtataContext_Artifacts() =>
            AtataContext.Current.Artifacts.FullName.Should.StartWith(_rootPath + Path.DirectorySeparatorChar);
    }
}
