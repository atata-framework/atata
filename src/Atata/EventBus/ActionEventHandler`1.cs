namespace Atata;

/// <summary>
/// Represents an event handler that executes an action.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public class ActionEventHandler<TEvent> : IEventHandler<TEvent>
{
    private readonly Action<TEvent, AtataContext> _action;

    public ActionEventHandler(Action action)
    {
        action.CheckNotNull(nameof(action));

        _action = (_, _) => action.Invoke();
    }

    public ActionEventHandler(Action<TEvent> action)
    {
        action.CheckNotNull(nameof(action));

        _action = (e, _) => action.Invoke(e);
    }

    public ActionEventHandler(Action<TEvent, AtataContext> action) =>
        _action = action.CheckNotNull(nameof(action));

    /// <inheritdoc/>
    public void Handle(TEvent eventData, AtataContext context) =>
        _action.Invoke(eventData, context);
}
