namespace Atata.Xunit;

public abstract class AtataGlobalFixture : AtataFixture
{
    protected AtataGlobalFixture()
        : base(AtataContextScope.Global)
    {
    }

    public override async ValueTask InitializeAsync()
    {
        ConfigureBaseConfiguration(AtataContext.BaseConfiguration);

        await base.InitializeAsync().ConfigureAwait(false);
    }

    protected virtual void ConfigureBaseConfiguration(AtataContextBuilder builder)
    {
    }

    protected override void ConfigureAtataContext(AtataContextBuilder builder) =>
        builder.UseTestSuiteType(GetType());
}
