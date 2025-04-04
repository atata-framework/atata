namespace Atata;

/// <summary>
/// Handles <typeparamref name="TEvent"/> events asynchronously.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public interface IAsyncEventHandler<in TEvent>
{
    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="context">The context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> object.</returns>
    Task HandleAsync(TEvent eventData, AtataContext context, CancellationToken cancellationToken);
}
