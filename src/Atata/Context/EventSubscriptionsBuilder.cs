namespace Atata;

/// <summary>
/// Represents the builder of event subscriptions.
/// </summary>
public class EventSubscriptionsBuilder
{
    public EventSubscriptionsBuilder() =>
        Items = [];

    public EventSubscriptionsBuilder(IEnumerable<EventSubscriptionItem> items) =>
        Items = items.ToList();

    /// <summary>
    /// Gets the list of event subscriptions.
    /// </summary>
    public List<EventSubscriptionItem> Items { get; }

    /// <summary>
    /// Adds the specified event handler as a subscription to the <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="eventHandler">The event handler.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder"/> instance.</returns>
    public EventSubscriptionsBuilder Add<TEvent>(Action eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public EventSubscriptionsBuilder Add<TEvent>(Action<TEvent> eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public EventSubscriptionsBuilder Add<TEvent>(Action<TEvent, AtataContext> eventHandler) =>
        Add(typeof(TEvent), new ActionEventHandler<TEvent>(eventHandler));

    /// <summary>
    /// Adds the created instance of <typeparamref name="TEventHandler"/> as a subscription to the <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    /// <returns>The same <see cref="EventSubscriptionsBuilder"/> instance.</returns>
    public EventSubscriptionsBuilder Add<TEvent, TEventHandler>()
        where TEventHandler : class, IEventHandler<TEvent>, new()
        =>
        Add(typeof(TEvent), new TEventHandler());

    /// <inheritdoc cref="Add{TEvent}(Action)"/>
    public EventSubscriptionsBuilder Add<TEvent>(IEventHandler<TEvent> eventHandler) =>
        Add(typeof(TEvent), eventHandler);

    /// <summary>
    /// Adds the created instance of <paramref name="eventHandlerType"/> as a subscription to the event type
    /// that is read from <see cref="IEventHandler{TEvent}"/> generic argument that <paramref name="eventHandlerType"/> should implement.
    /// </summary>
    /// <param name="eventHandlerType">Type of the event handler.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder"/> instance.</returns>
    public EventSubscriptionsBuilder Add(Type eventHandlerType)
    {
        eventHandlerType.CheckNotNull(nameof(eventHandlerType));

        Type expectedInterfaceType = typeof(IEventHandler<>);

        Type eventHanderType = eventHandlerType.GetGenericInterfaceType(expectedInterfaceType)
            ?? throw new ArgumentException($"'{nameof(eventHandlerType)}' of {eventHandlerType.FullName} type doesn't implement {expectedInterfaceType.FullName}.", nameof(eventHandlerType));

        Type eventType = eventHanderType.GetGenericArguments()[0];

        var eventHandler = ActivatorEx.CreateInstance(eventHandlerType);
        return Add(eventType, eventHandler);
    }

    /// <summary>
    /// Adds the created instance of <paramref name="eventHandlerType"/> as a subscription to the <paramref name="eventType"/>.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="eventHandlerType">Type of the event handler.</param>
    /// <returns>The same <see cref="EventSubscriptionsBuilder"/> instance.</returns>
    public EventSubscriptionsBuilder Add(Type eventType, Type eventHandlerType)
    {
        eventType.CheckNotNull(nameof(eventType));
        eventHandlerType.CheckNotNull(nameof(eventHandlerType));

        Type expectedType = typeof(IEventHandler<>).MakeGenericType(eventType);

        if (!expectedType.IsAssignableFrom(eventHandlerType))
            throw new ArgumentException($"'{nameof(eventHandlerType)}' of {eventHandlerType.FullName} type doesn't implement {expectedType.FullName}.", nameof(eventHandlerType));

        var eventHandler = ActivatorEx.CreateInstance(eventHandlerType);
        return Add(eventType, eventHandler);
    }

    private EventSubscriptionsBuilder Add(Type eventType, object eventHandler)
    {
        Items.Add(new EventSubscriptionItem(eventType, eventHandler));
        return this;
    }
}
