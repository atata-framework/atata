namespace Atata.Xunit;

public abstract class AtataGlobalFixture : AtataFixture
{
    protected AtataGlobalFixture()
        : base(AtataContextScope.Global)
    {
    }

    public override async Task InitializeAsync()
    {
        AtataContext.GlobalProperties.ModeOfCurrent = AtataContextModeOfCurrent.AsyncLocalBoxed;
        ConfigureAtataContextGlobalProperties(AtataContext.GlobalProperties);

        AtataContext.BaseConfiguration.UseAssertionExceptionFactory(XunitAssertionExceptionFactory.Instance);
        AtataContext.BaseConfiguration.UseAggregateAssertionExceptionFactory(XunitAggregateAssertionExceptionFactory.Instance);

        ConfigureAtataContextBaseConfiguration(AtataContext.BaseConfiguration);

        await base.InitializeAsync().ConfigureAwait(false);
    }

    protected virtual void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties)
    {
    }

    protected virtual void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
    }

    private protected sealed override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        builder.UseTestSuiteType(GetType());

        ConfigureGlobalAtataContext(builder);
    }

    protected virtual void ConfigureGlobalAtataContext(AtataContextBuilder builder)
    {
    }
}
