namespace Atata;

/// <summary>
/// Provides the <see cref="Context"/> property.
/// </summary>
/// <typeparam name="TContext">The type of the context.</typeparam>
#warning Remove IHasContext<out TContext> interface.
public interface IHasContext<out TContext>
{
    /// <summary>
    /// Gets the context.
    /// </summary>
    TContext Context { get; }
}
