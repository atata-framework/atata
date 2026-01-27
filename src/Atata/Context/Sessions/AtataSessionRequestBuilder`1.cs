namespace Atata;

/// <summary>
/// Represents a builder of a session request.
/// </summary>
/// <typeparam name="TBuilder">The type of the inherited builder class.</typeparam>
public abstract class AtataSessionRequestBuilder<TBuilder> : AtataSessionBuilderBase<TBuilder>, IAtataSessionProvider
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

    /// <summary>
    /// Gets or sets the count of sessions to request on startup.
    /// The default value is <c>1</c>.
    /// </summary>
    public int StartCount { get; set; } = 1;

    /// <summary>
    /// Gets or sets a value indicating whether to request multiple sessions in parallel on startup
    /// when <see cref="StartCount"/> is more than <c>1</c>.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool StartMultipleInParallel { get; set; } = true;

    /// <summary>
    /// Sets the <see cref="StartMultipleInParallel"/> value.
    /// </summary>
    /// <param name="enable">Enables parallel session requests if set to <see langword="true"/>.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStartMultipleInParallel(bool enable)
    {
        StartMultipleInParallel = enable;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="StartCount"/> value for a session request.
    /// </summary>
    /// <param name="count">The count of sessions to request on startup.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStartCount(int count)
    {
        Guard.ThrowIfLessThan(count, 1);

        StartCount = count;
        return (TBuilder)this;
    }

    async Task IAtataSessionProvider.StartAsync(AtataContext context, CancellationToken cancellationToken)
    {
        ValidateConfiguration();

        if (StartCount == 1)
        {
            await RequestSessionAsync(context, cancellationToken).ConfigureAwait(false);
        }
        else if (StartMultipleInParallel)
        {
            await RequestSessionsInParallelAsync(context, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            await RequestSessionsSequentiallyAsync(context, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task RequestSessionsInParallelAsync(AtataContext context, CancellationToken cancellationToken)
    {
        var buildTasks = new Task[StartCount];

        for (int i = 0; i < StartCount; i++)
        {
            var task = RequestSessionAsync(context, cancellationToken);
            buildTasks[i] = task;
        }

        await Task.WhenAll(buildTasks).ConfigureAwait(false);
    }

    private async Task RequestSessionsSequentiallyAsync(AtataContext context, CancellationToken cancellationToken)
    {
        for (int i = 0; i < StartCount; i++)
        {
            await RequestSessionAsync(context, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Requests a session asynchronously.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> object.</returns>
    protected abstract Task RequestSessionAsync(AtataContext context, CancellationToken cancellationToken);

    /// <summary>
    /// Validates the configuration.
    /// In case of invalid configuration the <see cref="AtataSessionRequestValidationException"/>
    /// can be thrown.
    /// </summary>
    protected virtual void ValidateConfiguration()
    {
        if (StartCount < 1)
            throw new AtataSessionRequestValidationException(
                $"Start count {StartCount} should be a positive value.");
    }
}
