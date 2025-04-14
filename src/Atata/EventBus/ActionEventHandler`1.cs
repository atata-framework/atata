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
        Guard.ThrowIfNull(action);

        _action = (_, _) => action.Invoke();
    }

    public ActionEventHandler(Action<TEvent> action)
    {
        Guard.ThrowIfNull(action);

        _action = (e, _) => action.Invoke(e);
    }

    public ActionEventHandler(Action<TEvent, AtataContext> action)
    {
        Guard.ThrowIfNull(action);

        _action = action;
    }

    /// <inheritdoc/>
    public void Handle(TEvent eventData, AtataContext context) =>
        _action.Invoke(eventData, context);
}
