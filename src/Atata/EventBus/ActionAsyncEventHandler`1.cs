namespace Atata;

/// <summary>
/// Represents an event handler that executes an asynchronous action.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public class ActionAsyncEventHandler<TEvent> : IAsyncEventHandler<TEvent>
{
    private readonly Func<TEvent, AtataContext, CancellationToken, Task> _action;

    public ActionAsyncEventHandler(Func<CancellationToken, Task> action)
    {
        action.CheckNotNull(nameof(action));

        _action = (_, _, ct) => action.Invoke(ct);
    }

    public ActionAsyncEventHandler(Func<TEvent, CancellationToken, Task> action)
    {
        action.CheckNotNull(nameof(action));

        _action = (e, _, ct) => action.Invoke(e, ct);
    }

    public ActionAsyncEventHandler(Func<TEvent, AtataContext, CancellationToken, Task> action) =>
        _action = action.CheckNotNull(nameof(action));

    /// <inheritdoc/>
    public Task HandleAsync(TEvent eventData, AtataContext context, CancellationToken cancellationToken) =>
        _action.Invoke(eventData, context, cancellationToken);
}
