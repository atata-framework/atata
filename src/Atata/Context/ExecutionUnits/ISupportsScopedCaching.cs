namespace Atata;

public interface ISupportsScopedCaching
{
    /// <summary>
    /// Executes the specified block action inside the scoped context with caching.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    void ExecuteScopedBlock(Action action);

    /// <summary>
    /// Executes the specified block function inside the scoped context with caching.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="function">The function to execute.</param>
    /// <returns>The result of the <paramref name="function"/>.</returns>
    TResult ExecuteScopedBlock<TResult>(Func<TResult> function);
}
