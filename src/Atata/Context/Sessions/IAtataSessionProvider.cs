#nullable enable

namespace Atata;

/// <summary>
/// An interface of an <see cref="AtataSession"/> provider.
/// </summary>
public interface IAtataSessionProvider : ICloneable
{
    /// <summary>
    /// Gets or sets the name of the session.
    /// The name may be <see langword="null"/>, non-unique, and can be used as a key to find a session.
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// Gets or sets the start scopes for which an <see cref="AtataSession"/> should automatically start.
    /// </summary>
    AtataSessionStartScopes? StartScopes { get; set; }

    /// <summary>
    /// Starts a session within the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> object.</returns>
    Task StartAsync(AtataContext context, CancellationToken cancellationToken);
}
