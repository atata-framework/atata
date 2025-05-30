namespace Atata.Xunit;

/// <summary>
/// Represents a base class for Atata Xunit test classes.
/// </summary>
public abstract class AtataFixture : IAsyncLifetime
{
    private readonly AtataContextScope _contextScope;

    protected AtataFixture(AtataContextScope contextScope)
    {
        _contextScope = contextScope;
        AtataContext.PresetCurrentAsyncLocalBox();
    }

    /// <summary>
    /// Gets the current <see cref="AtataContext"/> instance.
    /// </summary>
    public AtataContext Context { get; private set; } = null!;

    /// <summary>
    /// Initializes the <see cref="Context"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> object.</returns>
    public virtual async Task InitializeAsync()
    {
        AtataContextBuilder builder = AtataContext.CreateBuilder(_contextScope);

        ConfigureAtataContext(builder);

        Context = await builder.BuildAsync().ConfigureAwait(false);
    }

    private protected abstract void ConfigureAtataContext(AtataContextBuilder builder);

    /// <summary>
    /// Disposes the <see cref="Context"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> object.</returns>
    public virtual async Task DisposeAsync()
    {
        if (Context is not null)
            await Context.DisposeAsync().ConfigureAwait(false);
    }
}
