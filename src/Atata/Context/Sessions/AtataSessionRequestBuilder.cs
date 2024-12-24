#nullable enable

namespace Atata;

/// <summary>
/// Represents a builder of a session request.
/// </summary>
public abstract class AtataSessionRequestBuilder : IAtataSessionProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AtataSessionRequestBuilder"/> class.
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
    public AtataSessionStartScopes? StartScopes { get; set; }

    /// <summary>
    /// Sets the <see cref="Name"/> value for a session request.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The same <see cref="AtataSessionRequestBuilder"/> instance.</returns>
    public AtataSessionRequestBuilder UseName(string name)
    {
        Name = name;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="StartScopes"/> value for a session request.
    /// </summary>
    /// <param name="startScopes">The start scopes.</param>
    /// <returns>The same <see cref="AtataSessionRequestBuilder"/> instance.</returns>
    public AtataSessionRequestBuilder UseStartScopes(AtataSessionStartScopes? startScopes)
    {
        StartScopes = startScopes;
        return this;
    }

    Task IAtataSessionProvider.StartAsync(AtataContext context, CancellationToken cancellationToken) =>
        StartAsync(context, cancellationToken);

    protected abstract Task StartAsync(AtataContext context, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>The copied builder instance.</returns>
    public AtataSessionRequestBuilder Clone() =>
       (AtataSessionRequestBuilder)MemberwiseClone();

    object ICloneable.Clone() =>
        Clone();
}
