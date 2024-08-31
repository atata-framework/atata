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

    public bool IsActive =>
        throw new NotImplementedException();

    /// <summary>
    /// Gets the instance of the log manager associated with the session.
    /// </summary>
    public ILogManager Log { get; internal set; }

    /// <summary>
    /// Gets the event bus, which can used to subscribe to and publish events.
    /// </summary>
    protected IEventBus EventBus => Context.EventBus;

    public VariableHierarchicalDictionary Variables { get; internal set; }

    public StateHierarchicalDictionary State { get; internal set; }

    /// <summary>
    /// Gets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan BaseRetryTimeout { get; internal set; }

    /// <summary>
    /// Gets the base retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan BaseRetryInterval { get; internal set; }

    /// <summary>
    /// Gets the waiting timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan WaitingTimeout { get; internal set; }

    /// <summary>
    /// Gets the waiting retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan WaitingRetryInterval { get; internal set; }

    /// <summary>
    /// Gets the verification timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan VerificationTimeout { get; internal set; }

    /// <summary>
    /// Gets the verification retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan VerificationRetryInterval { get; internal set; }

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
