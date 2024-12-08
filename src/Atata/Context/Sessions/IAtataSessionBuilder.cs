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
    /// Gets or sets the target context to which the built session should be assigned to.
    /// Can be <see langword="null"/>.
    /// </summary>
    AtataContext TargetContext { get; set; }

    /// <summary>
    /// Gets or sets the start scopes for which an <see cref="AtataSession"/> should automatically start.
    /// </summary>
    AtataSessionStartScopes? StartScopes { get; set; }

    /// <summary>
    /// Gets or sets the start mode of the session.
    /// The default value is <see cref="AtataSessionStartMode.Build"/>.
    /// </summary>
    AtataSessionStartMode StartMode { get; set; }

    /// <summary>
    /// Builds the session within a target context, current context, or creates a temporary default context.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The built session.</returns>
    Task<AtataSession> BuildAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Builds the session within the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The built session.</returns>
    Task<AtataSession> BuildAsync(AtataContext context, CancellationToken cancellationToken = default);

    Task<AtataSession> StartAsync(CancellationToken cancellationToken = default);

    Task<AtataSession> StartAsync(AtataContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>The copied builder instance.</returns>
    IAtataSessionBuilder Clone();
}
