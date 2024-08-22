namespace Atata;

public abstract class AtataSession
{
    protected AtataSession(AtataContext context)
    {
        OwnerContext = Context = context.CheckNotNull(nameof(context));

        Variables = new(context.Variables);
        State = new(context.State);
    }

    public AtataContext OwnerContext { get; }

    public AtataContext Context { get; internal set; }

    public string Name { get; internal set; }

    /// <summary>
    /// Gets the instance of the log manager associated with the session.
    /// </summary>
    public ILogManager Log { get; internal set; }

    /// <summary>
    /// Gets the event bus, which can used to subscribe to and publish events.
    /// </summary>
    protected IEventBus EventBus => Context.EventBus;

    // TODO: Set initial variables, such as {session-name}.
    public VariableHierarchicalDictionary Variables { get; }

    public StateHierarchicalDictionary State { get; }

    public void SetAsCurrent() =>
        Context.Sessions.SetCurrent(this);

    public Task StartAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    internal void ReassignToContext(AtataContext context)
    {
        Context = context;
        Variables.ChangeParentDictionary(context.Variables);
        State.ChangeParentDictionary(context.State);
    }

    internal void ReassignToOwnerContext() =>
        ReassignToContext(OwnerContext);
}
