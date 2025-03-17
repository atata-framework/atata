namespace Atata.MSTest.IntegrationTests;

[TestClass]
public static class GlobalFixture
{
    [AssemblyInitialize]
    public static void SetUpAssembly(TestContext testContext)
    {
        Type globalFixtureType = typeof(GlobalFixture);

        AtataContext.GlobalProperties.UseRootNamespaceOf(globalFixtureType);

        AtataContext.BaseConfiguration.LogConsumers.AddNLogFile();

        MSTestGlobalAtataContextSetup.SetUp(globalFixtureType, testContext);
    }

    [AssemblyCleanup]
    public static void TearDownAssembly(TestContext testContext) =>
        MSTestGlobalAtataContextSetup.TearDown(testContext);
}
