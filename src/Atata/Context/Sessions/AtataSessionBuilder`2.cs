namespace Atata;

public abstract class AtataSessionBuilder<TSession, TBuilder> : IAtataSessionBuilder
    where TSession : AtataSession, new()
    where TBuilder : AtataSessionBuilder<TSession, TBuilder>
{
    public string Name { get; internal set; }

    public AtataSessionStart Start { get; internal set; }

    /// <summary>
    /// Gets the variables dictionary.
    /// </summary>
    public IDictionary<string, object> Variables { get; private set; } = new Dictionary<string, object>();

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
    /// which is equal to <c>500</c> milliseconds by default.
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
    /// which are equal to <c>500</c> milliseconds by default.
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
    /// which are equal to <c>500</c> milliseconds by default.
    /// </summary>
    public TimeSpan? VerificationRetryInterval { get; set; }

    /// <summary>
    /// Adds the variable.
    /// </summary>
    /// <param name="key">The variable key.</param>
    /// <param name="value">The variable value.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder AddVariable(string key, object value)
    {
        key.CheckNotNullOrWhitespace(nameof(key));

        Variables[key] = value;

        return (TBuilder)this;
    }

    /// <summary>
    /// Adds the variables.
    /// </summary>
    /// <param name="variables">The variables to add.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder AddVariables(IDictionary<string, object> variables)
    {
        variables.CheckNotNull(nameof(variables));

        foreach (var variable in variables)
            Variables[variable.Key] = variable.Value;

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
    /// The default value is <c>500</c> milliseconds.
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
    /// Sets the waiting timeout.
    /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryTimeout"/>, which is equal to <c>5</c> seconds by default.
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

    public AtataSession Build(AtataContext context)
    {
        TSession session = new();

        session.AssignToOwnerContext(context);
        ConfigureSession(session, context);

        return session;
    }

    protected virtual void ConfigureSession(TSession session, AtataContext context)
    {
        foreach (var variable in Variables)
            session.Variables.SetInitialValue(variable.Key, variable.Value);

        session.BaseRetryTimeoutOptional = BaseRetryTimeout;
        session.BaseRetryIntervalOptional = BaseRetryInterval;
        session.WaitingTimeoutOptional = WaitingTimeout;
        session.WaitingRetryIntervalOptional = WaitingRetryInterval;
        session.VerificationTimeoutOptional = VerificationTimeout;
        session.VerificationRetryIntervalOptional = VerificationRetryInterval;
    }

    object ICloneable.Clone()
    {
        var copy = (TBuilder)MemberwiseClone();

        OnClone(copy);

        return copy;
    }

    protected virtual void OnClone(TBuilder copy) =>
        copy.Variables = new Dictionary<string, object>(Variables);
}
