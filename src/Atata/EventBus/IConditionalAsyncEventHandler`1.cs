#nullable enable

namespace Atata;

/// <summary>
/// Handles <typeparamref name="TEvent"/> events asynchronously that match a condition.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public interface IConditionalAsyncEventHandler<in TEvent> : IAsyncEventHandler<TEvent>
{
    /// <inheritdoc cref="IConditionalEventHandler{TEvent}.CanHandle(TEvent, AtataContext)"/>
    bool CanHandle(TEvent eventData, AtataContext context);
}
