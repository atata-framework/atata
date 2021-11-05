using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventHandlerMap = System.Collections.Concurrent.ConcurrentDictionary<object, object>;

namespace Atata
{
    /// <summary>
    /// Represents the event bus, which provides a functionality of subscribing to and publishing events.
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly AtataContext _context;

        private readonly ConcurrentDictionary<Type, EventHandlerMap> _subscriptionMap = new ConcurrentDictionary<Type, EventHandlerMap>();

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

            if (_subscriptionMap.TryGetValue(typeof(TEvent), out EventHandlerMap eventHandlers))
            {
                foreach (IEventHandler<TEvent> handler in eventHandlers.Values.ToArray())
                {
                    if (!(handler is IConditionalEventHandler<TEvent> conditionalEventHandler)
                        || conditionalEventHandler.CanHandle(eventData, _context))
                    {
                        _context.Log.ExecuteSection(
                            new ExecuteEventHandlerLogSection(eventData, handler),
                            () => handler.Handle(eventData, _context));
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

        private static EventHandlerMap CreateEventHandlerMap(Type eventType) =>
            new EventHandlerMap();

        private object Subscribe(Type eventType, object eventHandler)
        {
            eventHandler.CheckNotNull(nameof(eventHandler));

            object subscription = new object();

            EventHandlerMap eventHandlerMap = _subscriptionMap.GetOrAdd(eventType, CreateEventHandlerMap);
            eventHandlerMap[subscription] = eventHandler;

            return subscription;
        }

        /// <inheritdoc/>
        public void Unsubscribe(object subscription)
        {
            subscription.CheckNotNull(nameof(subscription));

            foreach (EventHandlerMap eventHandlerMap in _subscriptionMap.Values)
            {
                if (eventHandlerMap.TryRemove(subscription, out _))
                    return;
            }
        }

        /// <inheritdoc/>
        public void UnsubscribeHandler(object eventHandler)
        {
            eventHandler.CheckNotNull(nameof(eventHandler));

            foreach (EventHandlerMap eventHandlerMap in _subscriptionMap.Values)
            {
                foreach (var subscriptionEventHandlerPair in eventHandlerMap.ToArray())
                {
                    if (Equals(subscriptionEventHandlerPair.Value, eventHandler))
                        eventHandlerMap.TryRemove(subscriptionEventHandlerPair.Key, out _);
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

            if (_subscriptionMap.TryGetValue(eventType, out EventHandlerMap eventHandlers))
                eventHandlers.Clear();
        }
    }
}
