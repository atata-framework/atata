namespace Atata
{
    /// <summary>
    /// Handles <typeparamref name="TEvent"/> events that match a condition.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IConditionalEventHandler<in TEvent> : IEventHandler<TEvent>
    {
        /// <summary>
        /// Determines whether this instance can or should handle the specified event.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified event; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandle(TEvent eventData, AtataContext context);
    }
}
