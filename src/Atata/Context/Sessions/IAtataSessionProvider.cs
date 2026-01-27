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
    AtataContextScopes? StartScopes { get; set; }

    /// <summary>
    /// Gets the collection of predicates that must be satisfied in order a session to start.
    /// </summary>
    List<Func<AtataContext, bool>> StartConditions { get; }

    /// <summary>
    /// Gets or sets the count of start sessions.
    /// The default value is <c>1</c>.
    /// </summary>
    int StartCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to start multiple sessions in parallel.
    /// The default value is <see langword="true"/>.
    /// </summary>
    bool StartMultipleInParallel { get; set; }

    /// <summary>
    /// Starts a session within the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> object.</returns>
    Task StartAsync(AtataContext context, CancellationToken cancellationToken);
}
