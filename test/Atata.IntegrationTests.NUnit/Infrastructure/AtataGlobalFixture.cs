using NUnit.Framework;

namespace Atata.NUnit;

[SetUpFixture]
public abstract class AtataGlobalFixture
{
    protected AtataContext Context { get; private set; }

    [OneTimeSetUp]
    public async Task SetUpGlobalAtataContextAsync()
    {
        AtataContext.BaseConfiguration.UseAllNUnitFeatures();
        ConfigureAtataContextBaseConfiguration(AtataContext.BaseConfiguration);

        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Global);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync(TestContext.CurrentContext.CancellationToken).ConfigureAwait(false);
    }

    [OneTimeTearDown]
    public async Task TearDownGlobalAtataContextAsync()
    {
        if (Context is not null)
            await Context.DisposeAsync().ConfigureAwait(false);
    }

    protected virtual void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
