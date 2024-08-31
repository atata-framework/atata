namespace Atata;

public abstract class AtataSession
{
    public AtataContext OwnerContext { get; private set; }

    public AtataContext Context { get; private set; }

    public string Name { get; internal set; }

    public bool IsActive =>
        throw new NotImplementedException();

    /// <summary>
    /// Gets the instance of the log manager associated with the session.
    /// </summary>
    public ILogManager Log { get; private set; }

    /// <summary>
    /// Gets the event bus, which can used to subscribe to and publish events.
    /// </summary>
    protected IEventBus EventBus => Context.EventBus;

    public VariableHierarchicalDictionary Variables { get; private set; }

    public StateHierarchicalDictionary State { get; private set; }

    /// <summary>
    /// Gets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan BaseRetryTimeout =>
        BaseRetryTimeoutOptional ?? Context.BaseRetryTimeout;

    /// <summary>
    /// Gets the base retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan BaseRetryInterval =>
        BaseRetryIntervalOptional ?? Context.BaseRetryInterval;

    /// <summary>
    /// Gets the waiting timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan WaitingTimeout =>
        WaitingTimeoutOptional ?? BaseRetryTimeoutOptional ?? Context.WaitingTimeout;

    /// <summary>
    /// Gets the waiting retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan WaitingRetryInterval =>
        WaitingRetryIntervalOptional ?? BaseRetryTimeoutOptional ?? Context.WaitingRetryInterval;

    /// <summary>
    /// Gets the verification timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan VerificationTimeout =>
        VerificationTimeoutOptional ?? BaseRetryTimeoutOptional ?? Context.VerificationTimeout;

    /// <summary>
    /// Gets the verification retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan VerificationRetryInterval =>
        VerificationRetryIntervalOptional ?? BaseRetryTimeoutOptional ?? Context.VerificationRetryInterval;

    internal TimeSpan? BaseRetryTimeoutOptional { get; set; }

    internal TimeSpan? BaseRetryIntervalOptional { get; set; }

    internal TimeSpan? WaitingTimeoutOptional { get; set; }

    internal TimeSpan? WaitingRetryIntervalOptional { get; set; }

    internal TimeSpan? VerificationTimeoutOptional { get; set; }

    internal TimeSpan? VerificationRetryIntervalOptional { get; set; }

    public void SetAsCurrent() =>
        Context.Sessions.SetCurrent(this);

    public Task StartAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    internal void AssignToOwnerContext(AtataContext context)
    {
        OwnerContext = context;
        ReassignToContext(context);
    }

    internal void ReassignToContext(AtataContext context)
    {
        Context = context;
        AssignToContext(context);
    }

    internal void ReassignToOwnerContext() =>
        ReassignToContext(OwnerContext);

    protected virtual void AssignToContext(AtataContext context)
    {
        Log = ((LogManager)context.Log).ForSession(this);

        if (Variables is null)
        {
            Variables = new(context.Variables);
            Variables.SetInitialValue("session-name", Name);
        }
        else
        {
            Variables.ChangeParentDictionary(context.Variables);
        }

        if (State is null)
            State = new(context.State);
        else
            State.ChangeParentDictionary(context.State);
    }
}
