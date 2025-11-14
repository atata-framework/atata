namespace Atata;

/// <summary>
/// Provides a builder for configuring the session pool settings.
/// </summary>
public sealed class AtataSessionPoolBuilder
{
    private readonly IAtataSessionBuilder _sessionBuilder;

    internal AtataSessionPoolBuilder(IAtataSessionBuilder sessionBuilder) =>
        _sessionBuilder = sessionBuilder;

    /// <summary>
    /// Sets the session pool initial capacity.
    /// The default value is <c>0</c>.
    /// </summary>
    /// <param name="capacity">The pool initial capacity.</param>
    /// <returns>The same <see cref="AtataSessionPoolBuilder"/> instance.</returns>
    public AtataSessionPoolBuilder WithInitialCapacity(int capacity)
    {
        _sessionBuilder.PoolInitialCapacity = capacity;
        return this;
    }

    /// <summary>
    /// Sets the session pool maximum capacity.
    /// The default value is <see cref="int.MaxValue"/>.
    /// </summary>
    /// <param name="capacity">The pool maximum capacity.</param>
    /// <returns>The same <see cref="AtataSessionPoolBuilder"/> instance.</returns>
    public AtataSessionPoolBuilder WithMaxCapacity(int capacity)
    {
        _sessionBuilder.PoolMaxCapacity = capacity;
        return this;
    }
}
