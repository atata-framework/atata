namespace Atata;

public abstract class AtataSession : IDisposable
{
    private bool _disposed;

    protected AtataSession() =>
        Id = AtataContext.GlobalProperties.IdGenerator.GenerateId();

    public AtataContext OwnerContext { get; private set; }

    public AtataContext Context { get; private set; }

    /// <summary>
    /// Gets the unique session identifier.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the name of the session.
    /// Returns <see langword="null"/> if the name is not set explicitly in builder.
    /// </summary>
    public string Name { get; internal set; }

#warning Temporarily IsActive is set to true by default.
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Gets the instance of the log manager associated with the session.
    /// </summary>
    public ILogManager Log { get; private set; }

    /// <summary>
    /// Gets the event bus of session,
    /// which can used to subscribe to and publish events.
    /// </summary>
    public IEventBus EventBus { get; internal set; }

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
        WaitingRetryIntervalOptional ?? BaseRetryIntervalOptional ?? Context.WaitingRetryInterval;

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
        VerificationRetryIntervalOptional ?? BaseRetryIntervalOptional ?? Context.VerificationRetryInterval;

    internal TimeSpan? BaseRetryTimeoutOptional { get; set; }

    internal TimeSpan? BaseRetryIntervalOptional { get; set; }

    internal TimeSpan? WaitingTimeoutOptional { get; set; }

    internal TimeSpan? WaitingRetryIntervalOptional { get; set; }

    internal TimeSpan? VerificationTimeoutOptional { get; set; }

    internal TimeSpan? VerificationRetryIntervalOptional { get; set; }

    public void SetAsCurrent() =>
        Context.Sessions.SetCurrent(this);

    public void Dispose()
    {
        if (_disposed)
            return;

        Log.ExecuteSection(
            new AtataSessionDeInitLogSection(this),
            () => Dispose(true));

        Variables.Clear();
        Log = null;

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            OwnerContext = null;
            Context = null;
            State.Clear();
            EventBus.UnsubscribeAll();
            IsActive = false;
        }
    }

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

    protected internal abstract Task StartAsync(CancellationToken cancellationToken = default);

    internal void Deactivate()
    {
        if (IsActive)
        {
            EventBus.Publish(new AtataSessionDeInitEvent(this));

#warning Review Deactivate method. Probably there is no need to set IsActive to false, but just reassign to onwer context, if present; otherwise dispose is needed.
            IsActive = false;
        }
    }

    protected internal virtual void LogConfiguration()
    {
    }

    protected virtual void AssignToContext(AtataContext context)
    {
        Log = ((LogManager)context.Log).ForSession(this);

        if (Variables is null)
        {
            Variables = new(context.Variables);
            Variables.SetInitialValue("execution-unit-id", Id);
            Variables.SetInitialValue("session-id", Id);
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
