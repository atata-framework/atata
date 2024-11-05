using NUnit.Framework;

namespace Atata.NUnit;

[Parallelizable(ParallelScope.Self)]
public abstract class AtataTestSuite
{
    protected AtataContext SuiteContext { get; private set; }

    protected AtataContext Context { get; private set; }

    [OneTimeSetUp]
    public async Task SetUpSuiteAtataContextAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.TestSuite);

        ConfigureSuiteAtataContext(builder);

        SuiteContext = await builder.BuildAsync(TestContext.CurrentContext.CancellationToken).ConfigureAwait(false);
    }

    [OneTimeTearDown]
    public async Task TearDownSuiteAtataContextAsync()
    {
        if (SuiteContext is not null)
            await SuiteContext.DisposeAsync().ConfigureAwait(false);
    }

    [SetUp]
    public async Task SetUpTestAtataContextAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.Test);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync(TestContext.CurrentContext.CancellationToken).ConfigureAwait(false);
    }

    [TearDown]
    public async Task TearDownTestAtataContextAsync()
    {
        if (Context is not null)
            await Context.DisposeAsync().ConfigureAwait(false);
    }

    protected virtual void ConfigureSuiteAtataContext(AtataContextBuilder builder)
    {
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
