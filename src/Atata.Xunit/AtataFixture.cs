namespace Atata.Xunit;

public abstract class AtataFixture : IAsyncLifetime
{
    private readonly AtataContextScope _contextScope;

    protected AtataFixture(AtataContextScope contextScope)
    {
        _contextScope = contextScope;
        AtataContext.PresetCurrentAsyncLocalBox();
    }

    public AtataContext Context { get; private set; } = null!;

    public virtual async Task InitializeAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(_contextScope);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync().ConfigureAwait(false);
    }

    protected virtual void ConfigureAtataContext(AtataContextBuilder builder)
    {
    }

    public virtual async Task DisposeAsync()
    {
        if (Context is not null)
            await Context.DisposeAsync().ConfigureAwait(false);
    }
}
