namespace Atata.NUnit;

/// <summary>
/// Represents a base class for Atata NUnit global fixture,
/// providing configuration and initialization for the global <see cref="AtataContext"/>,
/// base configuration of <see cref="AtataContext"/>,
/// configuration of global properties of <see cref="AtataContext"/>.
/// </summary>
[SetUpFixture]
public abstract class AtataGlobalFixture
{
    /// <summary>
    /// Gets the global <see cref="AtataContext"/> instance.
    /// </summary>
    protected AtataContext Context { get; private set; } = null!;

    /// <summary>
    /// Configures the <see cref="AtataContext.GlobalProperties"/>;
    /// configures the <see cref="AtataContext.BaseConfiguration"/>;
    /// configures and builds the global <see cref="AtataContext"/>.
    /// </summary>
    [OneTimeSetUp]
    public void SetUpGlobalAtataContext()
    {
        ConfigureAtataContextGlobalProperties(AtataContext.GlobalProperties);

        AtataContext.BaseConfiguration
            .UseNUnitTestName()
            .UseNUnitTestSuiteName()
            .UseNUnitTestSuiteType()
            .UseNUnitTestTraits()
            .UseNUnitAssertionExceptionFactory()
            .UseNUnitAggregateAssertionStrategy()
            .UseNUnitWarningReportStrategy()
            .UseNUnitAssertionFailureReportStrategy()
            .LogConsumers.AddNUnitTestContext()
            .EventSubscriptions.AddArtifactsToNUnitTestContext();

        ConfigureAtataContextBaseConfiguration(AtataContext.BaseConfiguration);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Global)
            .UseDefaultCancellationToken(TestExecutionContext.CurrentContext.CancellationToken);

        ConfigureGlobalAtataContext(builder);

        Context = builder.Build();
    }

    /// <summary>
    /// Tears down the global <see cref="AtataContext"/> after all tests have run.
    /// </summary>
    [OneTimeTearDown]
    public void TearDownGlobalAtataContext() =>
        NUnitAtataContextCompletionHandler.Complete(Context);

    /// <summary>
    /// Configures the global properties of the <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="globalProperties">The global properties to configure.</param>
    protected virtual void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties)
    {
    }

    /// <summary>
    /// Configures the base configuration of the <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The builder for the base configuration.</param>
    protected virtual void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
    }

    /// <summary>
    /// Configures the global <see cref="AtataContext"/>.
    /// The method can be overridden to provide custom configuration.
    /// </summary>
    /// <param name="builder">The builder for the global context.</param>
    protected virtual void ConfigureGlobalAtataContext(AtataContextBuilder builder)
    {
    }
}
