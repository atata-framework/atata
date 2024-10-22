namespace Atata;

/// <summary>
/// An interface of an <see cref="AtataSession"/> builder.
/// </summary>
public interface IAtataSessionBuilder
{
    /// <summary>
    /// Gets or sets the name of the session.
    /// The name may be non-unique and can be used as a key to find a session.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets the start scopes for which an <see cref="AtataSession"/> should automatically start.
    /// </summary>
    AtataSessionStartScopes? StartScopes { get; set; }

    Task<AtataSession> BuildAsync(AtataContext context);

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>The copied builder instance.</returns>
    IAtataSessionBuilder Clone();
}
