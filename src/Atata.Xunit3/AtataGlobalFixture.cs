namespace Atata.Xunit;

public abstract class AtataGlobalFixture : AtataFixture
{
    protected AtataGlobalFixture()
        : base(AtataContextScope.Global)
    {
    }

    public override async ValueTask InitializeAsync()
    {
        AtataContext.GlobalProperties.ModeOfCurrent = AtataContextModeOfCurrent.AsyncLocalBoxed;
        ConfigureAtataContextGlobalProperties(AtataContext.GlobalProperties);

        ConfigureAtataContextBaseConfiguration(AtataContext.BaseConfiguration);

        await base.InitializeAsync().ConfigureAwait(false);
    }

    protected virtual void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties)
    {
    }

    protected virtual void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
    }

    protected override void ConfigureAtataContext(AtataContextBuilder builder) =>
        builder.UseTestSuiteType(GetType());
}
