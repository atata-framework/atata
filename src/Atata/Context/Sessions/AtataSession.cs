namespace Atata;

public abstract class AtataSession : IDisposable
{
    private bool _disposed;

    protected AtataSession()
    {
        Id = AtataContext.GlobalProperties.IdGenerator.GenerateId();
        ExecutionUnit = new AtataSessionExecutionUnit(this);
    }

    public AtataContext OwnerContext { get; private set; }

    public AtataContext Context { get; private set; }

    /// <summary>
    /// Gets the execution unit.
    /// </summary>
    public IAtataExecutionUnit ExecutionUnit { get; }

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
    /// Gets the <see cref="IReport{TOwner}"/> instance that provides a reporting functionality.
    /// </summary>
    public IReport<AtataSession> Report { get; internal set; }

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

    /// <summary>
    /// Executes an aggregate assertion.
    /// </summary>
    /// <param name="action">The action to execute in scope of aggregate assertion.</param>
    /// <param name="assertionScopeName">
    /// Name of the scope being asserted.
    /// Is used to identify the assertion section in log.
    /// Can be <see langword="null"/>.
    /// </param>
    public void AggregateAssert(Action action, string assertionScopeName = null) =>
        Context.AggregateAssert(action, Log, assertionScopeName);

    /// <inheritdoc cref="AtataContext.RaiseAssertionError(string, Exception)"/>
    public void RaiseAssertionError(string message, Exception exception = null) =>
       AssertionVerificationStrategy.Instance.ReportFailure(ExecutionUnit, message, exception);

    /// <inheritdoc cref="AtataContext.RaiseAssertionWarning(string, Exception)"/>
    public void RaiseAssertionWarning(string message, Exception exception = null) =>
        ExpectationVerificationStrategy.Instance.ReportFailure(ExecutionUnit, message, exception);

    public void Dispose()
    {
        if (_disposed)
            return;

        Log.ExecuteSection(
            new AtataSessionDeInitLogSection(this),
            () => Dispose(true));

        Variables.Clear();
        Log = null;
        Report = null;

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

    /// <summary>
    /// Returns a string that represents the current session.
    /// </summary>
    /// <returns>
    /// A <see langword="string"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        StringBuilder messageBuilder = new StringBuilder(GetType().Name)
            .Append(" { Id=")
            .Append(Id);

        if (!string.IsNullOrEmpty(Name))
            messageBuilder.Append(", Name=")
                .Append(Name);

        messageBuilder.Append(" }");

        return messageBuilder.ToString();
    }
}
