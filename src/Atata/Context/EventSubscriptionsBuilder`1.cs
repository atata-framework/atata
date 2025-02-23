#nullable enable

namespace Atata;

/// <summary>
/// Represents the builder of event subscriptions.
/// </summary>
/// <typeparam name="TRootBuilder">The type of the root builder.</typeparam>
public sealed class EventSubscriptionsBuilder<TRootBuilder>
{
    private readonly TRootBuilder _rootBuilder;

    private readonly List<EventSubscriptionItem> _items;

    internal EventSubscriptionsBuilder(
        TRootBuilder rootBuilder,
        List<EventSubscriptionItem> items)
    {
        _rootBuilder = rootBuilder;
        _items = items;
    }

    /// <summary>
    /// Gets the list of event subscriptions.
    /// </summary>
    public IReadOnlyList<EventSubscriptionItem> Items =>
        _items;

#pragma warning disable CA2263 // Prefer generic overload when type is known

    /// <summary>
    /// Adds the specified event handler as a subscription to the <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="eventHandler">The event handler.</param>
    /// <returns>The <typeparamref name="TRootBuilder"/> instance.</returns>
    public TRootBuilder Add<TEvent>(Action eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public TRootBuilder Add<TEvent>(Action<TEvent> eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public TRootBuilder Add<TEvent>(Action<TEvent, AtataContext> eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public TRootBuilder Add<TEvent>(Func<CancellationToken, Task> eventHandler) =>
        Add(typeof(TEvent), new ActionAsyncEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public TRootBuilder Add<TEvent>(Func<TEvent, CancellationToken, Task> eventHandler) =>
        Add(typeof(TEvent), new ActionAsyncEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public TRootBuilder Add<TEvent>(Func<TEvent, AtataContext, CancellationToken, Task> eventHandler) =>
        Add(typeof(TEvent), new ActionAsyncEventHandler<TEvent>(eventHandler));

    /// <summary>
    /// Adds the created instance of <typeparamref name="TEventHandler"/> as a subscription to the <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler that should be either <see cref="IEventHandler{TEvent}"/> or <see cref="IAsyncEventHandler{TEvent}"/>.</typeparam>
    /// <returns>The <typeparamref name="TRootBuilder"/> instance.</returns>
    public TRootBuilder Add<TEvent, TEventHandler>()
        where TEventHandler : class, new()
    {
        ValidateEventHandlerType(typeof(TEventHandler), typeof(TEvent));
        return Add(typeof(TEvent), new TEventHandler());
    }

#pragma warning restore CA2263 // Prefer generic overload when type is known

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public TRootBuilder Add<TEvent>(IEventHandler<TEvent> eventHandler) =>
        Add(typeof(TEvent), eventHandler);

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public TRootBuilder Add<TEvent>(IAsyncEventHandler<TEvent> eventHandler) =>
        Add(typeof(TEvent), eventHandler);

    /// <summary>
    /// Adds the created instance of <paramref name="eventHandlerType"/> as a subscription to the event type
    /// that is read from <see cref="IEventHandler{TEvent}"/> generic argument that <paramref name="eventHandlerType"/> should implement.
    /// </summary>
    /// <param name="eventHandlerType">Type of the event handler.</param>
    /// <returns>The <typeparamref name="TRootBuilder"/> instance.</returns>
    public TRootBuilder Add(Type eventHandlerType)
    {
        eventHandlerType.CheckNotNull(nameof(eventHandlerType));

        Type expectedSyncInterfaceType = typeof(IEventHandler<>);
        Type expectedAsyncInterfaceType = typeof(IAsyncEventHandler<>);

        Type eventHanderType = eventHandlerType.GetGenericInterfaceType(expectedSyncInterfaceType)
            ?? eventHandlerType.GetGenericInterfaceType(expectedAsyncInterfaceType)
            ?? throw new ArgumentException(
                $"'{nameof(eventHandlerType)}' of {eventHandlerType.FullName} type doesn't implement {expectedSyncInterfaceType.FullName} or {expectedAsyncInterfaceType.FullName}.",
                nameof(eventHandlerType));

        Type eventType = eventHanderType.GetGenericArguments()[0];

        var eventHandler = ActivatorEx.CreateInstance(eventHandlerType);
        return Add(eventType, eventHandler);
    }

    /// <summary>
    /// Adds the created instance of <paramref name="eventHandlerType"/> as a subscription to the <paramref name="eventType"/>.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="eventHandlerType">Type of the event handler.</param>
    /// <returns>The <typeparamref name="TRootBuilder"/> instance.</returns>
    public TRootBuilder Add(Type eventType, Type eventHandlerType)
    {
        eventType.CheckNotNull(nameof(eventType));
        eventHandlerType.CheckNotNull(nameof(eventHandlerType));

        ValidateEventHandlerType(eventHandlerType, eventType);
        var eventHandler = ActivatorEx.CreateInstance(eventHandlerType);

        return Add(eventType, eventHandler);
    }

    private static void ValidateEventHandlerType(Type eventHandlerType, Type eventType)
    {
        Type expectedSyncHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        Type expectedAsyncHandlerType = typeof(IAsyncEventHandler<>).MakeGenericType(eventType);

        if (!expectedSyncHandlerType.IsAssignableFrom(eventHandlerType) && !expectedAsyncHandlerType.IsAssignableFrom(eventHandlerType))
            throw new ArgumentException(
                $"'{nameof(eventHandlerType)}' of {eventHandlerType.FullName} type doesn't implement {expectedSyncHandlerType.FullName} or {expectedAsyncHandlerType.FullName}.",
                nameof(eventHandlerType));
    }

    private TRootBuilder Add(Type eventType, object eventHandler)
    {
        _items.Add(new(eventType, eventHandler));
        return _rootBuilder;
    }
}
