using Xunit;

namespace Atata.Xunit;

public abstract class AtataFixture : IAsyncLifetime
{
    private readonly AtataContextScope _contextScope;

    protected AtataFixture(AtataContextScope contextScope)
    {
        _contextScope = contextScope;
        AtataContext.PresetCurrentAsyncLocalBox();
    }

    public AtataContext Context { get; private set; }

    public virtual async ValueTask InitializeAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(_contextScope);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync(TestContext.Current.CancellationToken).ConfigureAwait(false);
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (Context is not null)
            await Context.DisposeAsync().ConfigureAwait(false);
    }
}
