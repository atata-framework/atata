namespace Atata;

/// <summary>
/// An interface of an <see cref="AtataSession"/> builder.
/// </summary>
public interface IAtataSessionBuilder : IAtataSessionProvider
{
    /// <summary>
    /// Gets or sets the target context to which the built session should be assigned to.
    /// Can be <see langword="null"/>.
    /// </summary>
    AtataContext? TargetContext { get; set; }

    /// <summary>
    /// Gets or sets the session operating mode.
    /// The default value is <see cref="AtataSessionMode.Own"/>.
    /// </summary>
    AtataSessionMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the session pool initial capacity.
    /// The default value is <c>0</c>.
    /// Applies when <see cref="Mode"/> is set to <see cref="AtataSessionMode.Pool"/>.
    /// </summary>
    int PoolInitialCapacity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to fill in parallel the session pool
    /// when <see cref="PoolInitialCapacity"/> is more than <c>1</c>.
    /// The default value is <see langword="true"/>.
    /// Applies when <see cref="Mode"/> is set to <see cref="AtataSessionMode.Pool"/>.
    /// </summary>
    bool PoolFillInParallel { get; set; }

    /// <summary>
    /// Gets or sets the session pool maximum capacity.
    /// The default value is <see cref="int.MaxValue"/>.
    /// Applies when <see cref="Mode"/> is set to <see cref="AtataSessionMode.Pool"/>.
    /// </summary>
    int PoolMaxCapacity { get; set; }

    /// <summary>
    /// Gets or sets the session waiting timeout,
    /// which is used in session borrowing and getting from pool.
    /// The default value is <c>5</c> minutes.
    /// </summary>
    TimeSpan SessionWaitingTimeout { get; set; }

    /// <summary>
    /// Gets or sets the session waiting retry interval,
    /// which is used in session borrowing and getting from pool.
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    TimeSpan SessionWaitingRetryInterval { get; set; }

    /// <summary>
    /// Builds the session within a target <see cref="AtataContext"/>,
    /// <see cref="AtataContext.Current"/>,
    /// or creates a temporary default non-scoped context.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The built session.</returns>
    Task<AtataSession> BuildAsync(CancellationToken cancellationToken = default);
}
