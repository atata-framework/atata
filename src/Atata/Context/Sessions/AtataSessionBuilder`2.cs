#nullable enable

namespace Atata;

public abstract class AtataSessionBuilder<TSession, TBuilder> : IAtataSessionBuilder
    where TSession : AtataSession, new()
    where TBuilder : AtataSessionBuilder<TSession, TBuilder>
{
    private AtataContext? _targetContext;

    protected AtataSessionBuilder() =>
        EventSubscriptions = new((TBuilder)this);

    /// <inheritdoc/>
    public string? Name { get; set; }

    AtataContext? IAtataSessionBuilder.TargetContext
    {
        get => _targetContext;
        set => _targetContext = value;
    }

    /// <inheritdoc/>
    public AtataContextScopes? StartScopes { get; set; }

    /// <inheritdoc/>
    public AtataSessionMode Mode { get; set; }

    /// <inheritdoc/>
    public int PoolInitialCapacity { get; set; } =
        AtataSession.DefaultPoolInitialCapacity;

    /// <inheritdoc/>
    public int PoolMaxCapacity { get; set; } =
        AtataSession.DefaultPoolMaxCapacity;

    /// <inheritdoc/>
    public bool PoolFillInParallel { get; set; } =
        true;

    /// <summary>
    /// Gets the variables dictionary.
    /// </summary>
    public IDictionary<string, object> Variables { get; private set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets the state dictionary.
    /// </summary>
    public IDictionary<string, object> State { get; private set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the base retry timeout for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="AtataContext.BaseRetryTimeout"/>,
    /// which is equal to <c>5</c> seconds by default.
    /// </summary>
    public TimeSpan? BaseRetryTimeout { get; set; }

    /// <summary>
    /// Gets or sets the base retry interval for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="AtataContext.BaseRetryInterval"/>,
    /// which is equal to <c>200</c> milliseconds by default.
    /// </summary>
    public TimeSpan? BaseRetryInterval { get; set; }

    /// <summary>
    /// Gets or sets the waiting timeout for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="BaseRetryTimeout"/> or <see cref="AtataContext.WaitingTimeout"/>,
    /// which are equal to <c>5</c> seconds by default.
    /// </summary>
    public TimeSpan? WaitingTimeout { get; set; }

    /// <summary>
    /// Gets or sets the waiting retry interval for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="BaseRetryTimeout"/> or <see cref="AtataContext.WaitingRetryInterval"/>,
    /// which are equal to <c>200</c> milliseconds by default.
    /// </summary>
    public TimeSpan? WaitingRetryInterval { get; set; }

    /// <summary>
    /// Gets or sets the verification timeout for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="BaseRetryTimeout"/> or <see cref="AtataContext.VerificationTimeout"/>,
    /// which are equal to <c>5</c> seconds by default.
    /// </summary>
    public TimeSpan? VerificationTimeout { get; set; }

    /// <summary>
    /// Gets or sets the verification retry interval for session.
    /// The default value is <see langword="null"/>.
    /// When <see langword="null"/>, the value for session will be taken from
    /// <see cref="BaseRetryTimeout"/> or <see cref="AtataContext.VerificationRetryInterval"/>,
    /// which are equal to <c>200</c> milliseconds by default.
    /// </summary>
    public TimeSpan? VerificationRetryInterval { get; set; }

    /// <inheritdoc/>
    public TimeSpan SessionWaitingTimeout { get; set; } =
        TimeSpan.FromMinutes(5);

    /// <inheritdoc/>
    public TimeSpan SessionWaitingRetryInterval { get; set; } =
        TimeSpan.FromMilliseconds(200);

    /// <summary>
    /// Gets the builder of event subscriptions,
    /// which provides the methods to subscribe to Atata and custom events.
    /// </summary>
    public AtataSessionEventSubscriptionsBuilder<TBuilder> EventSubscriptions { get; private set; }

    /// <summary>
    /// Sets the <see cref="Name"/> value for a session.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseName(string? name)
    {
        Name = name;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="StartScopes"/> value for a session.
    /// </summary>
    /// <param name="startScopes">The start scopes.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStartScopes(AtataContextScopes? startScopes)
    {
        StartScopes = startScopes;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="StartScopes"/> value for a session
    /// with either <see cref="AtataContextScopes.All"/> or <see cref="AtataContextScopes.None"/>,
    /// depending on the <paramref name="start"/> parameter.
    /// </summary>
    /// <param name="start">Whether to start the session.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseStart(bool start = true)
    {
        StartScopes = start
            ? AtataContextScopes.All
            : AtataContextScopes.None;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="Mode"/> value for a session.
    /// </summary>
    /// <param name="mode">The mode.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseMode(AtataSessionMode mode)
    {
        Mode = mode;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the variable.
    /// </summary>
    /// <param name="key">The variable key.</param>
    /// <param name="value">The variable value.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseVariable(string key, object value)
    {
        key.CheckNotNullOrWhitespace(nameof(key));

        Variables[key] = value;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the variables.
    /// </summary>
    /// <param name="variables">The variables to set.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseVariables(IEnumerable<KeyValuePair<string, object>> variables)
    {
        variables.CheckNotNull(nameof(variables));

        foreach (var variable in variables)
            Variables[variable.Key] = variable.Value;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the state object.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The state value.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseState<TValue>(TValue value)
    {
        State[StateHierarchicalDictionary.ResolveTypeKey<TValue>()] = value!;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the state object.
    /// </summary>
    /// <param name="key">The state key.</param>
    /// <param name="value">The state value.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseState(string key, object value)
    {
        key.CheckNotNullOrWhitespace(nameof(key));

        State[key] = value;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the state objects.
    /// </summary>
    /// <param name="objects">The objects to set.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseState(IEnumerable<KeyValuePair<string, object>> objects)
    {
        objects.CheckNotNull(nameof(objects));

        foreach (var variable in objects)
            State[variable.Key] = variable.Value;

        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="BaseRetryTimeout"/> value.
    /// Sets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseBaseRetryTimeout(TimeSpan? timeout)
    {
        BaseRetryTimeout = timeout;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="BaseRetryInterval"/> value.
    /// Sets the base retry interval.
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseBaseRetryInterval(TimeSpan? interval)
    {
        BaseRetryInterval = interval;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="WaitingTimeout"/> value.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseWaitingTimeout(TimeSpan? timeout)
    {
        WaitingTimeout = timeout;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="WaitingRetryInterval"/> value.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseWaitingRetryInterval(TimeSpan? interval)
    {
        WaitingRetryInterval = interval;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="VerificationTimeout"/> value.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseVerificationTimeout(TimeSpan? timeout)
    {
        VerificationTimeout = timeout;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="VerificationRetryInterval"/> value.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseVerificationRetryInterval(TimeSpan? interval)
    {
        VerificationRetryInterval = interval;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="SessionWaitingTimeout"/> value.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseSessionWaitingTimeout(TimeSpan timeout)
    {
        SessionWaitingTimeout = timeout;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="SessionWaitingRetryInterval"/> value.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseSessionWaitingRetryInterval(TimeSpan interval)
    {
        SessionWaitingRetryInterval = interval;
        return (TBuilder)this;
    }

    async Task IAtataSessionProvider.StartAsync(AtataContext context, CancellationToken cancellationToken)
    {
        if (Mode == AtataSessionMode.Pool)
        {
            await context.Sessions.StartPoolAsync(this, cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            await BuildAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc cref="IAtataSessionBuilder.BuildAsync(CancellationToken)"/>
    public async Task<TSession> BuildAsync(CancellationToken cancellationToken = default)
    {
        var assignToContext = _targetContext ?? AtataContext.Current;

        if (assignToContext is not null)
        {
            return await BuildAsync(assignToContext, cancellationToken).ConfigureAwait(false);
        }
        else if (Mode is not (AtataSessionMode.Own or AtataSessionMode.Shared))
        {
            throw new InvalidOperationException(
                $"Cannot start session with {nameof(Mode)} value {Mode} without {nameof(AtataContext)}.");
        }
        else
        {
            var contextBuilder = AtataContext.CreateDefaultNonScopedBuilder();

            contextBuilder.Sessions.Add(this);

            var context = await contextBuilder.BuildAsync(cancellationToken).ConfigureAwait(false);

            var session = context.Sessions.Get<TSession>();
            session.DisposesThroughContext = true;
            return session;
        }
    }

    private async Task<TSession> BuildAsync(AtataContext context, CancellationToken cancellationToken = default)
    {
        ValidateConfiguration();

        TSession session = new()
        {
            Name = Name,
            Mode = Mode,
            IsShareable = Mode == AtataSessionMode.Shared
        };

        session.AssignToOwnerContext(context);

        await session.Log.ExecuteSectionAsync(
            new AtataSessionInitLogSection(session),
            async () =>
            {
                ConfigureSession(session);

                await session.EventBus.PublishAsync(new AtataSessionInitStartedEvent(session), cancellationToken)
                    .ConfigureAwait(false);

                session.LogConfiguration();

                await session.StartAsync(cancellationToken).ConfigureAwait(false);

                await session.EventBus.PublishAsync(new AtataSessionInitCompletedEvent(session), cancellationToken)
                    .ConfigureAwait(false);
                await session.EventBus.PublishAsync(new AtataSessionAssignedToContextEvent(session), cancellationToken)
                    .ConfigureAwait(false);
            }).ConfigureAwait(false);

        return session;
    }

    async Task<AtataSession> IAtataSessionBuilder.BuildAsync(CancellationToken cancellationToken) =>
        await BuildAsync(cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Validates the configuration.
    /// In case of invalid configuration the <see cref="AtataSessionBuilderValidationException"/>
    /// can be thrown.
    /// </summary>
    protected virtual void ValidateConfiguration()
    {
    }

    protected virtual void ConfigureSession(TSession session)
    {
        session.Report = CreateReport(session);

        foreach (var variable in Variables)
            session.Variables.SetInitialValue(variable.Key, variable.Value);

        foreach (var stateObject in State)
            session.State.SetInitialValue(stateObject.Key, stateObject.Value);

        session.BaseRetryTimeoutOptional = BaseRetryTimeout;
        session.BaseRetryIntervalOptional = BaseRetryInterval;
        session.WaitingTimeoutOptional = WaitingTimeout;
        session.WaitingRetryIntervalOptional = WaitingRetryInterval;
        session.VerificationTimeoutOptional = VerificationTimeout;
        session.VerificationRetryIntervalOptional = VerificationRetryInterval;
        session.SessionWaitingTimeout = SessionWaitingTimeout;
        session.SessionWaitingRetryInterval = SessionWaitingRetryInterval;

        session.EventBus = new EventBus(session.Context, EventSubscriptions.Items);
    }

    protected virtual IReport<TSession> CreateReport(TSession session) =>
        new Report<TSession>(session, session.ExecutionUnit);

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>The copied builder instance.</returns>
    public TBuilder Clone()
    {
        var copy = (TBuilder)MemberwiseClone();
        copy._targetContext = null;

        OnClone(copy);

        return copy;
    }

    object ICloneable.Clone() =>
        Clone();

    protected virtual void OnClone(TBuilder copy)
    {
        copy.Variables = new Dictionary<string, object>(Variables);
        copy.State = new Dictionary<string, object>(State);

        copy.EventSubscriptions = EventSubscriptions.CloneFor(copy);
    }
}
