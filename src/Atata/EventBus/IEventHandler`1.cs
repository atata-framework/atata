#nullable enable

namespace Atata;

/// <summary>
/// Handles <typeparamref name="TEvent"/> events.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public interface IEventHandler<in TEvent>
{
    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="context">The context.</param>
    void Handle(TEvent eventData, AtataContext context);
}
