namespace Atata.NUnit;

[SetUpFixture]
public abstract class AtataGlobalFixture
{
    protected AtataContext Context { get; private set; } = null!;

    [OneTimeSetUp]
    public void SetUpGlobalAtataContext()
    {
        ConfigureAtataContextGlobalProperties(AtataContext.GlobalProperties);

        AtataContext.BaseConfiguration
            .UseNUnitTestName()
            .UseNUnitTestSuiteName()
            .UseNUnitTestSuiteType()
            .UseNUnitAssertionExceptionType()
            .UseNUnitAggregateAssertionStrategy()
            .UseNUnitWarningReportStrategy()
            .UseNUnitAssertionFailureReportStrategy();

        AtataContext.BaseConfiguration.LogConsumers
            .AddNUnitTestContext();

        AtataContext.BaseConfiguration.EventSubscriptions
            .AddArtifactsToNUnitTestContext();

        ConfigureAtataContextBaseConfiguration(AtataContext.BaseConfiguration);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Global);

        ConfigureAtataContext(builder);

        Context = builder.Build(TestContext.CurrentContext.CancellationToken);
    }

    [OneTimeTearDown]
    public void TearDownGlobalAtataContext() =>
        TestCompletionHandler.CompleteTest(Context);

    protected virtual void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties)
    {
    }

    protected virtual void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
