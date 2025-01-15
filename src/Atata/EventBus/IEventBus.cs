#nullable enable

namespace Atata;

/// <summary>
/// Provides a functionality of subscribing to and publishing events.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes the specified event to subscribed event handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="eventData">The event data.</param>
    void Publish<TEvent>(TEvent eventData);

    /// <summary>
    /// Publishes the specified event to subscribed event handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="eventData">The event data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> object.</returns>
    Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes the specified event handler to the <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="eventHandler">The event handler.</param>
    /// <returns>A subscription object.</returns>
    object Subscribe<TEvent>(Action eventHandler);

    /// <inheritdoc cref="Subscribe{TEvent}(Action)"/>
    object Subscribe<TEvent>(Action<TEvent> eventHandler);

    /// <inheritdoc cref="Subscribe{TEvent}(Action)"/>
    object Subscribe<TEvent>(Action<TEvent, AtataContext> eventHandler);

    /// <inheritdoc cref="Subscribe{TEvent}(Action)"/>
    object Subscribe<TEvent>(Func<CancellationToken, Task> eventHandler);

    /// <inheritdoc cref="Subscribe{TEvent}(Action)"/>
    object Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> eventHandler);

    /// <inheritdoc cref="Subscribe{TEvent}(Action)"/>
    object Subscribe<TEvent>(Func<TEvent, AtataContext, CancellationToken, Task> eventHandler);

    /// <summary>
    /// Subscribes the created instance of <typeparamref name="TEventHandler"/> to the <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    /// <returns>A subscription object.</returns>
    object Subscribe<TEvent, TEventHandler>()
        where TEventHandler : class, new();

    /// <inheritdoc cref="Subscribe{TEvent}(Action)"/>
    object Subscribe<TEvent>(IEventHandler<TEvent> eventHandler);

    /// <inheritdoc cref="Subscribe{TEvent}(Action)"/>
    object Subscribe<TEvent>(IAsyncEventHandler<TEvent> eventHandler);

    /// <summary>
    /// Removes the specified subscription.
    /// </summary>
    /// <param name="subscription">The subscription object.</param>
    void Unsubscribe(object subscription);

    /// <summary>
    /// Removes the event handler instance from subscriptions.
    /// </summary>
    /// <param name="eventHandler">The event handler.</param>
    void UnsubscribeHandler(object eventHandler);

    /// <summary>
    /// Removes all subscriptions from the specified <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    void UnsubscribeAll<TEvent>();

    /// <summary>
    /// Removes all subscriptions from the specified event type.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    void UnsubscribeAll(Type eventType);

    /// <summary>
    /// Removes all subscriptions.
    /// </summary>
    void UnsubscribeAll();
}
