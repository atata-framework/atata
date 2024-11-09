using NUnit.Framework;

namespace Atata.NUnit;

[SetUpFixture]
public abstract class AtataGlobalFixture
{
    protected AtataContext Context { get; private set; }

    [OneTimeSetUp]
    public async Task SetUpGlobalAtataContextAsync()
    {
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
            .TakeScreenshotOnNUnitError()
            .TakePageSnapshotOnNUnitError()
            .AddArtifactsToNUnitTestContext();

        ConfigureAtataContextBaseConfiguration(AtataContext.BaseConfiguration);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Global);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync(TestContext.CurrentContext.CancellationToken).ConfigureAwait(false);
    }

    [OneTimeTearDown]
    public async Task TearDownGlobalAtataContextAsync() =>
        await TestCompletionHandler.CompleteTestAsync(Context).ConfigureAwait(false);

    protected virtual void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
