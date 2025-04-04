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

    internal EventBus(AtataContext context, IEnumerable<EventSubscriptionItem>? eventSubscriptions)
    {
        _context = context.CheckNotNull(nameof(context));

        if (eventSubscriptions is not null)
            foreach (var subscription in eventSubscriptions)
                Subscribe(subscription.EventType, subscription.EventHandler);
    }

    /// <inheritdoc/>
    public void Publish<TEvent>(TEvent eventData)
    {
        eventData.CheckNotNull(nameof(eventData));

        if (_subscriptionMap.TryGetValue(typeof(TEvent), out var eventHandlerSubscriptions)
            && eventHandlerSubscriptions.Count != 0)
        {
            object[] eventHandlersArray;

            lock (eventHandlerSubscriptions)
            {
                eventHandlersArray = [.. eventHandlerSubscriptions.Select(x => x.EventHandler)];
            }

            PublishToEventHandlersAsync(eventData, eventHandlersArray).RunSync();
        }
    }

    public async Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default)
    {
        eventData.CheckNotNull(nameof(eventData));

        if (_subscriptionMap.TryGetValue(typeof(TEvent), out var eventHandlerSubscriptions)
            && eventHandlerSubscriptions.Count != 0)
        {
            object[] eventHandlersArray;

            lock (eventHandlerSubscriptions)
            {
                eventHandlersArray = [.. eventHandlerSubscriptions.Select(x => x.EventHandler)];
            }

            await PublishToEventHandlersAsync(eventData, eventHandlersArray, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task PublishToEventHandlersAsync<TEvent>(TEvent eventData, object[] eventHandlers, CancellationToken cancellationToken = default)
    {
        List<Task> executingTasks = [];

        foreach (object handler in eventHandlers)
        {
            if (handler is IEventHandler<TEvent> syncHandler)
            {
                if (syncHandler is not IConditionalEventHandler<TEvent> conditionalSyncHandler
                    || conditionalSyncHandler.CanHandle(eventData, _context))
                {
                    syncHandler.Handle(eventData, _context);
                }
            }
            else if (handler is IAsyncEventHandler<TEvent> asyncHandler)
            {
                if (asyncHandler is not IConditionalAsyncEventHandler<TEvent> conditionalAsyncHandler
                    || conditionalAsyncHandler.CanHandle(eventData, _context))
                {
                    Task task = asyncHandler.HandleAsync(eventData, _context, cancellationToken);

                    executingTasks.Add(task);
                }
            }
        }

        if (executingTasks.Count > 0)
            await Task.WhenAll(executingTasks).ConfigureAwait(false);
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
    public object Subscribe<TEvent>(Func<CancellationToken, Task> eventHandler)
    {
        eventHandler.CheckNotNull(nameof(eventHandler));

        return Subscribe(new ActionAsyncEventHandler<TEvent>(eventHandler));
    }

    /// <inheritdoc/>
    public object Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> eventHandler)
    {
        eventHandler.CheckNotNull(nameof(eventHandler));

        return Subscribe(new ActionAsyncEventHandler<TEvent>(eventHandler));
    }

    /// <inheritdoc/>
    public object Subscribe<TEvent>(Func<TEvent, AtataContext, CancellationToken, Task> eventHandler)
    {
        eventHandler.CheckNotNull(nameof(eventHandler));

        return Subscribe(new ActionAsyncEventHandler<TEvent>(eventHandler));
    }

    /// <inheritdoc/>
    public object Subscribe<TEvent, TEventHandler>()
        where TEventHandler : class, new()
        =>
        new TEventHandler() switch
        {
            IEventHandler<TEvent> syncHandler => Subscribe(syncHandler),
            IAsyncEventHandler<TEvent> asyncHandler => Subscribe(asyncHandler),
            _ => throw new InvalidOperationException(
                $"Event handler type {typeof(TEventHandler).FullName} cannot be subscribed, " +
                $"because it doesn't implement neither {typeof(IEventHandler<TEvent>).FullName} nor {typeof(IAsyncEventHandler<TEvent>).FullName}.")
        };

    /// <inheritdoc/>
    public object Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) =>
        Subscribe(typeof(TEvent), eventHandler);

    /// <inheritdoc/>
    public object Subscribe<TEvent>(IAsyncEventHandler<TEvent> eventHandler) =>
        Subscribe(typeof(TEvent), eventHandler);

    private static List<EventHandlerSubscription> CreateEventHandlerSubscriptionList(Type eventType) =>
        [];

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

    /// <inheritdoc/>
    public void UnsubscribeAll() =>
        _subscriptionMap.Clear();

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
