namespace Atata;

/// <summary>
/// Represents the event subscription.
/// </summary>
public class EventSubscriptionItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventSubscriptionItem"/> class.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <param name="eventHandler">The event handler, which should implement <see cref="IEventHandler{TEvent}"/>.</param>
    public EventSubscriptionItem(Type eventType, object eventHandler)
    {
        Guard.ThrowIfNull(eventType);
        Guard.ThrowIfNull(eventHandler);

        EventType = eventType;
        EventHandler = eventHandler;
    }

    /// <summary>
    /// Gets the type of the event.
    /// </summary>
    public Type EventType { get; }

    /// <summary>
    /// Gets the event handler, which should implement <see cref="IEventHandler{TEvent}"/>.
    /// </summary>
    public object EventHandler { get; }
}
