﻿namespace Atata.NUnit;

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
            .UseNUnitAssertionExceptionFactory()
            .UseNUnitAggregateAssertionStrategy()
            .UseNUnitWarningReportStrategy()
            .UseNUnitAssertionFailureReportStrategy()
            .LogConsumers.AddNUnitTestContext()
            .EventSubscriptions.AddArtifactsToNUnitTestContext();

        ConfigureAtataContextBaseConfiguration(AtataContext.BaseConfiguration);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Global);

        ConfigureGlobalAtataContext(builder);

        Context = builder.Build(TestContext.CurrentContext.CancellationToken);
    }

    [OneTimeTearDown]
    public void TearDownGlobalAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(Context);

    protected virtual void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties)
    {
    }

    protected virtual void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
    }

    protected virtual void ConfigureGlobalAtataContext(AtataContextBuilder builder)
    {
    }
}
