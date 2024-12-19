namespace Atata;

public abstract class AtataSession : IAsyncDisposable
{
    private bool _isDisposed;

    protected AtataSession()
    {
        Id = AtataContext.GlobalProperties.IdGenerator.GenerateId();
        ExecutionUnit = new AtataSessionExecutionUnit(this);
    }

    public AtataContext OwnerContext { get; private set; }

    public AtataContext Context { get; private set; }

    public bool IsBorrowed => Context != OwnerContext;

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
    public string Name { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the session is active (not disposed).
    /// </summary>
    public bool IsActive =>
        !_isDisposed;

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
                                EventBus.Publish(new AtataSessionDeInitEvent(this));
                            }
                            finally
                            {
                                await DisposeAsyncCore().ConfigureAwait(false);

                                EventBus.Publish(new AtataSessionDeInitCompletedEvent(this));
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
                Log = null;
                Report = null;

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

        OwnerContext = null;
        Context = null;

        State.Clear();
        EventBus.UnsubscribeAll();
        return default;
    }

    internal bool TryBorrowTo(AtataContext context)
    {
        if (!IsBorrowed)
        {
            lock (BorrowLock)
            {
                if (!IsBorrowed)
                {
                    Log.Trace($"{this} borrowed by {context}");
                    context.Sessions.Add(this);
                    AssignToContext(context);
                    Log.Trace($"{this} borrowed from {OwnerContext}");
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the session to the owner context from which the session was borrowed.
    /// </summary>
    public void ReturnToOwnerContext()
    {
        if (IsBorrowed)
        {
            var currentContext = Context;

            Log.Trace($"{this} returned to {OwnerContext}");

            AssignToContext(OwnerContext);
            currentContext.Sessions.Remove(this);

            Log.Trace($"{this} returned by {currentContext}");
        }
    }

    internal void AssignToOwnerContext(AtataContext context)
    {
        OwnerContext = context;
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

    internal static string BuildTypedName(Type sessionType, string sessionName)
    {
        string sessionTypeName = sessionType.Name;

        return string.IsNullOrEmpty(sessionName)
            ? sessionTypeName
            : $"{sessionTypeName} {{ Name={sessionName} }}";
    }
}
