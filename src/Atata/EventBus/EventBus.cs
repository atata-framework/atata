namespace Atata;

/// <summary>
/// Represents the event bus, which provides a functionality of subscribing to and publishing events.
/// </summary>
public class EventBus : IEventBus
{
    private readonly AtataContext _context;

    private readonly ConcurrentDictionary<Type, List<EventHandlerSubscription>> _subscriptionMap = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EventBus"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public EventBus(AtataContext context)
        : this(context, null)
    {
    }

    internal EventBus(AtataContext context, IEnumerable<EventSubscriptionItem> eventSubscriptions)
    {
        _context = context.CheckNotNull(nameof(context));

        if (eventSubscriptions != null)
            foreach (var subscription in eventSubscriptions)
                Subscribe(subscription.EventType, subscription.EventHandler);
    }

    /// <inheritdoc/>
    public void Publish<TEvent>(TEvent eventData)
    {
        eventData.CheckNotNull(nameof(eventData));

        if (_subscriptionMap.TryGetValue(typeof(TEvent), out var eventHandlerSubscriptions))
        {
            object[] eventHandlersArray;

            lock (eventHandlerSubscriptions)
            {
                eventHandlersArray = eventHandlerSubscriptions.Select(x => x.EventHandler).ToArray();
            }

            foreach (IEventHandler<TEvent> handler in eventHandlersArray.Cast<IEventHandler<TEvent>>())
            {
                if (handler is not IConditionalEventHandler<TEvent> conditionalEventHandler
                    || conditionalEventHandler.CanHandle(eventData, _context))
                {
                    handler.Handle(eventData, _context);
                }
            }
        }
    }

    /// <inheritdoc/>
    public object Subscribe<TEvent>(Action eventHandler)
    {
        eventHandler.CheckNotNull(nameof(eventHandler));

        return Subscribe(new ActionEventHandler<TEvent>(eventHandler));
    }

    /// <inheritdoc/>
    public object Subscribe<TEvent>(Action<TEvent> eventHandler)
    {
        eventHandler.CheckNotNull(nameof(eventHandler));

        return Subscribe(new ActionEventHandler<TEvent>(eventHandler));
    }

    /// <inheritdoc/>
    public object Subscribe<TEvent>(Action<TEvent, AtataContext> eventHandler)
    {
        eventHandler.CheckNotNull(nameof(eventHandler));

        return Subscribe(new ActionEventHandler<TEvent>(eventHandler));
    }

    /// <inheritdoc/>
    public object Subscribe<TEvent, TEventHandler>()
        where TEventHandler : class, IEventHandler<TEvent>, new()
        =>
        Subscribe(new TEventHandler());

    /// <inheritdoc/>
    public object Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) =>
        Subscribe(typeof(TEvent), eventHandler);

    private static List<EventHandlerSubscription> CreateEventHandlerSubscriptionList(Type eventType) =>
        new();

    private object Subscribe(Type eventType, object eventHandler)
    {
        eventHandler.CheckNotNull(nameof(eventHandler));

        object subscription = new object();

        var eventHandlerSubscriptions = _subscriptionMap.GetOrAdd(eventType, CreateEventHandlerSubscriptionList);

        lock (eventHandlerSubscriptions)
        {
            eventHandlerSubscriptions.Add(new EventHandlerSubscription(subscription, eventHandler));
        }

        return subscription;
    }

    /// <inheritdoc/>
    public void Unsubscribe(object subscription)
    {
        subscription.CheckNotNull(nameof(subscription));

        foreach (var eventHandlerSubscriptions in _subscriptionMap.Values)
        {
            lock (eventHandlerSubscriptions)
            {
                if (eventHandlerSubscriptions.RemoveAll(x => x.SubscriptionObject == subscription) > 0)
                    return;
            }
        }
    }

    /// <inheritdoc/>
    public void UnsubscribeHandler(object eventHandler)
    {
        eventHandler.CheckNotNull(nameof(eventHandler));

        foreach (var eventHandlerSubscriptions in _subscriptionMap.Values)
        {
            lock (eventHandlerSubscriptions)
            {
                eventHandlerSubscriptions.RemoveAll(x => Equals(x.EventHandler, eventHandler));
            }
        }
    }

    /// <inheritdoc/>
    public void UnsubscribeAll<TEvent>() =>
        UnsubscribeAll(typeof(TEvent));

    /// <inheritdoc/>
    public void UnsubscribeAll(Type eventType)
    {
        eventType.CheckNotNull(nameof(eventType));

        if (_subscriptionMap.TryGetValue(eventType, out var eventHandlerSubscriptions))
        {
            lock (eventHandlerSubscriptions)
            {
                eventHandlerSubscriptions.Clear();
            }
        }
    }

    private sealed class EventHandlerSubscription
    {
        public EventHandlerSubscription(object subscriptionObject, object eventHandler)
        {
            SubscriptionObject = subscriptionObject;
            EventHandler = eventHandler;
        }

        public object SubscriptionObject { get; }

        public object EventHandler { get; }
    }
}
