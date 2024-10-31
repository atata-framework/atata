using Xunit;

namespace Atata.Xunit;

public abstract class AtataFixture : IAsyncLifetime
{
    private readonly AtataContextScope _contextScope;

    protected AtataFixture(AtataContextScope contextScope) =>
        _contextScope = contextScope;

    public AtataContext Context { get; private set; }

    public virtual async ValueTask InitializeAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(_contextScope);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync();
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }

    public virtual async ValueTask DisposeAsync() =>
        await Context.DisposeAsync().ConfigureAwait(false);
}
