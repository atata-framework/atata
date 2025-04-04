namespace Atata;

public abstract class AtataSession : IAsyncDisposable
{
    internal const int DefaultPoolInitialCapacity = 0;

    internal const int DefaultPoolMaxCapacity = int.MaxValue;

    private bool _isDisposed;

    protected AtataSession()
    {
        Id = AtataContext.GlobalProperties.IdGenerator.GenerateId();
        ExecutionUnit = new AtataSessionExecutionUnit(this);
    }

    /// <summary>
    /// Gets the owner <see cref="AtataContext"/>, the context in which this session was created.
    /// </summary>
    public AtataContext OwnerContext { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the borrow source <see cref="AtataContext"/>, the context from which this session was borrowed.
    /// Initially equals to <see langword="null"/>.
    /// </summary>
    /// <value>
    /// The borrow source context.
    /// </value>
    private AtataContext? BorrowSourceContext { get; set; }

    /// <summary>
    /// Gets the <see cref="AtataContext"/> that this session is currently associated with.
    /// Initially equals to <see cref="OwnerContext"/>, but changes when borrowed or taken from a pool.
    /// </summary>
    public AtataContext Context { get; private set; } = null!;

    public AtataSessionMode Mode { get; internal set; }

    public bool IsShareable { get; internal set; }

    public bool IsBorrowed =>
        IsShareable && BorrowSourceContext != null;

    public bool IsTakenFromPool { get; internal set; }

    public bool IsBorrowedOrTakenFromPool => IsTakenFromPool || IsBorrowed;

    internal object BorrowLock { get; } = new object();

    internal bool DisposesThroughContext { get; set; }

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
    public string? Name { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the session is active (not disposed).
    /// </summary>
    public bool IsActive =>
        !_isDisposed;

    /// <summary>
    /// Gets the instance of the log manager associated with the session.
    /// </summary>
    public ILogManager Log { get; private set; } = null!;

    /// <summary>
    /// Gets the <see cref="IReport{TOwner}"/> instance that provides a reporting functionality.
    /// </summary>
    public IReport<AtataSession> Report { get; internal set; } = null!;

    /// <summary>
    /// Gets the event bus of session,
    /// which can used to subscribe to and publish events.
    /// </summary>
    public IEventBus EventBus { get; internal set; } = null!;

    /// <summary>
    /// <para>
    /// Gets the state hierarchical dictionary of this session.
    /// </para>
    /// <para>
    /// List of predefined variables:
    /// </para>
    /// <list type="bullet">
    /// <item><c>artifacts</c></item>
    /// <item><c>test-name-sanitized</c></item>
    /// <item><c>test-name</c></item>
    /// <item><c>test-suite-name-sanitized</c></item>
    /// <item><c>test-suite-name</c></item>
    /// <item><c>test-start</c></item>
    /// <item><c>test-start-utc</c></item>
    /// <item><c>context-id</c></item>
    /// <item><c>execution-unit-id</c></item>
    /// <item><c>session-id</c></item>
    /// <item><c>session-name</c></item>
    /// </list>
    /// <para>
    /// Custom variables can be added as well.
    /// </para>
    /// </summary>
    public VariableHierarchicalDictionary Variables { get; private set; } = null!;

    /// <summary>
    /// Gets the state hierarchical dictionary of this session.
    /// By default the dictionary is empty.
    /// </summary>
    public StateHierarchicalDictionary State { get; private set; } = null!;

    /// <summary>
    /// Gets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan BaseRetryTimeout =>
        BaseRetryTimeoutOptional ?? Context.BaseRetryTimeout;

    /// <summary>
    /// Gets the base retry interval.
    /// The default value is <c>200</c> milliseconds.
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
    /// The default value is <c>200</c> milliseconds.
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
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    public TimeSpan VerificationRetryInterval =>
        VerificationRetryIntervalOptional ?? BaseRetryIntervalOptional ?? Context.VerificationRetryInterval;

    internal TimeSpan SessionWaitingTimeout { get; set; }

    internal TimeSpan SessionWaitingRetryInterval { get; set; }

    internal TimeSpan? BaseRetryTimeoutOptional { get; set; }

    internal TimeSpan? BaseRetryIntervalOptional { get; set; }

    internal TimeSpan? WaitingTimeoutOptional { get; set; }

    internal TimeSpan? WaitingRetryIntervalOptional { get; set; }

    internal TimeSpan? VerificationTimeoutOptional { get; set; }

    internal TimeSpan? VerificationRetryIntervalOptional { get; set; }

    /// <summary>
    /// Sets this session as current within the associated context.
    /// </summary>
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
    public void AggregateAssert(Action action, string? assertionScopeName = null) =>
        Context.AggregateAssert(action, Log, assertionScopeName);

    /// <inheritdoc cref="AtataContext.RaiseAssertionError(string, Exception)"/>
    public void RaiseAssertionError(string message, Exception? exception = null) =>
       AssertionVerificationStrategy.Instance.ReportFailure(ExecutionUnit, message, exception);

    /// <inheritdoc cref="AtataContext.RaiseAssertionWarning(string, Exception)"/>
    public void RaiseAssertionWarning(string message, Exception? exception = null) =>
        ExpectationVerificationStrategy.Instance.ReportFailure(ExecutionUnit, message, exception);

    public async ValueTask DisposeAsync()
    {
        if (!_isDisposed)
        {
            if (DisposesThroughContext)
            {
                DisposesThroughContext = false;
                await Context.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                await Log.ExecuteSectionAsync(
                    new AtataSessionDeInitLogSection(this),
                    async () =>
                    {
                        try
                        {
                            try
                            {
                                await EventBus.PublishAsync(new AtataSessionDeInitStartedEvent(this))
                                    .ConfigureAwait(false);
                            }
                            finally
                            {
                                await DisposeAsyncCore().ConfigureAwait(false);

                                await EventBus.PublishAsync(new AtataSessionDeInitCompletedEvent(this))
                                    .ConfigureAwait(false);

                                EventBus.UnsubscribeAll();
                                State.Clear();
                            }
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception, "Session deinitialization failed.");
                            throw;
                        }
                    })
                    .ConfigureAwait(false);

                Variables.Clear();
                Log = null!;
                Report = null!;

                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }

    protected virtual ValueTask DisposeAsyncCore()
    {
        Context.Sessions.Remove(this);

        if (OwnerContext != Context)
            OwnerContext.Sessions.Remove(this);

        OwnerContext = null!;
        Context = null!;
        BorrowSourceContext = null;
        IsTakenFromPool = false;

        return default;
    }

    internal bool TryBorrowTo(AtataContext context)
    {
        if (IsShareable && !IsBorrowed)
        {
            lock (BorrowLock)
            {
                if (!IsBorrowed)
                {
                    var previousContext = Context;

                    if (context != previousContext)
                    {
                        Log.Trace($"{this} is borrowed by {context}");

                        context.Sessions.Add(this);
                        previousContext.Sessions.Remove(this);
                        BorrowSourceContext = previousContext;
                        ReassignToContext(context);

                        Log.Trace($"{this} is borrowed from {previousContext}");
                    }

                    return true;
                }
            }
        }

        return false;
    }

    internal void GiveFromPoolToContext(AtataContext context)
    {
        if (context != OwnerContext)
        {
            Log.Trace($"{this} is taken from pool by {context}");
            context.Sessions.Add(this);
            ReassignToContext(context);
            Log.Trace($"{this} is taken from pool of {OwnerContext}");
        }
        else
        {
            context.Sessions.Add(this);
            Log.Trace($"{this} is taken from pool");
        }
    }

    /// <summary>
    /// Returns the session to the owner context from which the session was borrowed.
    /// </summary>
    public void ReturnToSessionSource()
    {
        if (IsBorrowedOrTakenFromPool)
        {
            bool shouldReturnToPool = IsTakenFromPool && BorrowSourceContext is null;
            AtataContext currentContext = Context;
            AtataContext contextToReturnTo = BorrowSourceContext ?? OwnerContext;

            Log.Trace($"{this} is returned to{(shouldReturnToPool ? " pool of" : null)} {contextToReturnTo}");

            BorrowSourceContext = null;
            ReassignToContext(contextToReturnTo);
            currentContext.Sessions.Remove(this);

            if (shouldReturnToPool)
                OwnerContext.Sessions.GetPool(GetType(), Name).Return(this);
            else
                contextToReturnTo.Sessions.Add(this);

            Log.Trace($"{this} is returned{(shouldReturnToPool ? " to pool" : null)} by {currentContext}");
        }
    }

    internal void AssignToOwnerContext(AtataContext context)
    {
        OwnerContext = context;

        if (Mode != AtataSessionMode.Pool)
            context.Sessions.Add(this);

        AssignToContext(context);
    }

    protected internal abstract Task StartAsync(CancellationToken cancellationToken = default);

    protected internal virtual void LogConfiguration()
    {
    }

    protected internal virtual void TakeFailureSnapshot()
    {
    }

    protected internal virtual void OnAssignedToContext()
    {
    }

    private void ReassignToContext(AtataContext context)
    {
        AtataSessionUnassignedFromContextEvent unassignedEvent = new(this);
        EventBus.Publish(unassignedEvent);
        Context.EventBus.Publish(unassignedEvent);

        AssignToContext(context);

        AtataSessionAssignedToContextEvent assignedEvent = new(this);
        EventBus.Publish(assignedEvent);
        Context.EventBus.Publish(assignedEvent);
    }

    private void AssignToContext(AtataContext context)
    {
        Context = context;
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

        OnAssignedToContext();
    }

    /// <summary>
    /// Returns a string that represents the current session.
    /// </summary>
    /// <returns>
    /// A <see langword="string"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        var stringBuilder = new StringBuilder(GetType().Name)
            .Append(" { Id=")
            .Append(Id);

        if (!string.IsNullOrEmpty(Name))
            stringBuilder.Append(", Name=")
                .Append(Name);

        stringBuilder.Append(" }");

        return stringBuilder.ToString();
    }

    internal static string BuildTypedName(Type sessionType, string? sessionName)
    {
        string sessionTypeName = sessionType.Name;

        return string.IsNullOrEmpty(sessionName)
            ? sessionTypeName
            : $"{sessionTypeName} {{ Name={sessionName} }}";
    }
}
