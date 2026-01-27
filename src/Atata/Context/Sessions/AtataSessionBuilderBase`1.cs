namespace Atata;

/// <summary>
/// Represents a base builder for creating and configuring session providers.
/// </summary>
/// <typeparam name="TBuilder">The type of the inherited builder.</typeparam>
public abstract class AtataSessionBuilderBase<TBuilder> : ICloneable
    where TBuilder : AtataSessionBuilderBase<TBuilder>
{
    /// <inheritdoc cref="IAtataSessionProvider.Name"/>
    public string? Name { get; set; }

    /// <inheritdoc cref="IAtataSessionProvider.StartScopes"/>
    public AtataContextScopes? StartScopes { get; set; }

    /// <inheritdoc cref="IAtataSessionProvider.StartConditions"/>
    public List<Func<AtataContext, bool>> StartConditions { get; private set; } = [];

    /// <summary>
    /// Configures this builder by action delegate.
    /// </summary>
    /// <param name="configure">An action delegate to configure the <typeparamref name="TBuilder"/>.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder Use(Action<TBuilder> configure)
    {
        TBuilder thisCasted = (TBuilder)this;

        configure?.Invoke(thisCasted);
        return thisCasted;
    }

    /// <summary>
    /// Sets the <see cref="Name"/> value for a session.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseName(string? name)
    {
        Name = name;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="StartScopes"/> value for a session
    /// with either <see cref="AtataContextScopes.All"/> or <see cref="AtataContextScopes.None"/>,
    /// depending on the <paramref name="start"/> parameter.
    /// </summary>
    /// <param name="start">Whether to start the session.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStart(bool start = true)
    {
        StartScopes = start
            ? AtataContextScopes.All
            : AtataContextScopes.None;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="StartScopes"/> value for a session.
    /// </summary>
    /// <param name="startScopes">The start scopes.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStartScopes(AtataContextScopes? startScopes)
    {
        StartScopes = startScopes;
        return (TBuilder)this;
    }

    /// <summary>
    /// Adds a start condition predicate that determines whether the session should be started for the provided <see cref="AtataContext"/>.
    /// </summary>
    /// <param name="predicate">A predicate that returns <see langword="true"/> to allow starting the session.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStartCondition(Func<AtataContext, bool> predicate)
    {
        if (predicate is not null)
            StartConditions.Add(predicate);

        return (TBuilder)this;
    }

    /// <inheritdoc cref="UseStartCondition(Func{AtataContext, bool})"/>
    public TBuilder UseStartCondition(Func<AtataContext, Task<bool>> predicate)
    {
        if (predicate is not null)
            StartConditions.Add(x => predicate.Invoke(x).RunSync());

        return (TBuilder)this;
    }

    /// <inheritdoc cref="UseStartCondition(Func{AtataContext, bool})"/>
    public TBuilder UseStartCondition(Func<AtataContext, ValueTask<bool>> predicate)
    {
        if (predicate is not null)
            StartConditions.Add(x => predicate.Invoke(x).RunSync());

        return (TBuilder)this;
    }

    /// <summary>
    /// Adds a start condition that verifies whether the specified TCP <paramref name="port"/> is available.
    /// The condition succeeds when the port is available.
    /// </summary>
    /// <param name="port">The TCP port to check for availability.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStartWhenPortIsAvailable(int port) =>
        UseStartCondition(_ => PortUtils.IsPortAvailable(port));

    /// <summary>
    /// Returns a string that represents the current session builder.
    /// </summary>
    /// <returns>
    /// A <see langword="string"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        string typeName = GetType().ToStringInShortForm();

        return Name?.Length > 0
            ? $"{typeName} {{ Name={Name} }}"
            : typeName;
    }

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>A copied builder instance.</returns>
    public TBuilder Clone()
    {
        var copy = (TBuilder)MemberwiseClone();

        OnClone(copy);

        return copy;
    }

    object ICloneable.Clone() =>
        Clone();

    /// <summary>
    /// Called when cloning the session provider to create a copy.
    /// Can be overridden in derived classes to add custom cloning logic.
    /// </summary>
    /// <param name="copy">The builder copy to be configured.</param>
    protected virtual void OnClone(TBuilder copy) =>
        copy.StartConditions = [.. StartConditions];
}
