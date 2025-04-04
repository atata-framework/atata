namespace Atata;

/// <summary>
/// Represents a builder of a session request.
/// </summary>
/// <typeparam name="TBuilder">The type of the inherited builder class.</typeparam>
public abstract class AtataSessionRequestBuilder<TBuilder> : IAtataSessionProvider
    where TBuilder : AtataSessionRequestBuilder<TBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AtataSessionRequestBuilder{TBuilder}"/> class.
    /// </summary>
    /// <param name="sessionType">Type of the session.</param>
    protected AtataSessionRequestBuilder(Type sessionType) =>
        Type = sessionType;

    /// <summary>
    /// Gets the type of session to request.
    /// </summary>
    public Type Type { get; }

    /// <inheritdoc/>
    public string? Name { get; set; }

    /// <inheritdoc/>
    public AtataContextScopes? StartScopes { get; set; }

    /// <summary>
    /// Sets the <see cref="Name"/> value for a session request.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseName(string? name)
    {
        Name = name;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="StartScopes"/> value for a session request.
    /// </summary>
    /// <param name="startScopes">The start scopes.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStartScopes(AtataContextScopes? startScopes)
    {
        StartScopes = startScopes;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="StartScopes"/> value for a session request
    /// with either <see cref="AtataContextScopes.All"/> or <see cref="AtataContextScopes.None"/>,
    /// depending on the <paramref name="start"/> parameter.
    /// </summary>
    /// <param name="start">Whether to start the session request.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStart(bool start = true)
    {
        StartScopes = start
            ? AtataContextScopes.All
            : AtataContextScopes.None;
        return (TBuilder)this;
    }

    Task IAtataSessionProvider.StartAsync(AtataContext context, CancellationToken cancellationToken) =>
        StartAsync(context, cancellationToken);

    protected abstract Task StartAsync(AtataContext context, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>The copied builder instance.</returns>
    public TBuilder Clone() =>
       (TBuilder)MemberwiseClone();

    object ICloneable.Clone() =>
        Clone();
}
