namespace Atata;

/// <summary>
/// Represents a builder of a session request.
/// </summary>
public sealed class AtataSessionRequestBuilder
{
    internal AtataSessionRequestBuilder(Type sessionType) =>
        Type = sessionType;

    /// <summary>
    /// Gets the type of session to request.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets or sets the name of the session to request.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the start scopes for which an <see cref="AtataSession"/> should automatically be requested.
    /// </summary>
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

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>The copied builder instance.</returns>
    public AtataSessionRequestBuilder Clone() =>
       (AtataSessionRequestBuilder)MemberwiseClone();
}
