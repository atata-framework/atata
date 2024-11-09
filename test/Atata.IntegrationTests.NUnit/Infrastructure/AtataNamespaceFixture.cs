using NUnit.Framework;

namespace Atata.NUnit;

[SetUpFixture]
public abstract class AtataNamespaceFixture
{
    protected AtataContext Context { get; private set; }

    [OneTimeSetUp]
    public async Task SetUpNamespaceAtataContextAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(AtataContextScope.NamespaceSuite);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync(TestContext.CurrentContext.CancellationToken).ConfigureAwait(false);
    }

    [OneTimeTearDown]
    public async Task TearDownNamespaceAtataContextAsync() =>
        await TestCompletionHandler.CompleteTestAsync(Context).ConfigureAwait(false);

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }
}
