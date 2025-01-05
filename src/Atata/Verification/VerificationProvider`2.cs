namespace Atata;

public abstract class VerificationProvider<TVerificationProvider, TOwner> : IVerificationProvider<TOwner>
    where TVerificationProvider : VerificationProvider<TVerificationProvider, TOwner>
{
    private const StringComparison DefaultStringComparison = StringComparison.Ordinal;

    private readonly bool _isNegation;

    protected VerificationProvider(IAtataExecutionUnit executionUnit, bool isNegation = false)
    {
        ExecutionUnit = executionUnit;
        _isNegation = isNegation;
    }

    bool IVerificationProvider<TOwner>.IsNegation => _isNegation;

    protected IVerificationStrategy Strategy { get; set; } = AssertionVerificationStrategy.Instance;

    IVerificationStrategy IVerificationProvider<TOwner>.Strategy
    {
        get => Strategy;
        set => Strategy = value;
    }

    TOwner IVerificationProvider<TOwner>.Owner => Owner;

    protected abstract TOwner Owner { get; }

    IAtataExecutionUnit IVerificationProvider<TOwner>.ExecutionUnit => ExecutionUnit;

    protected IAtataExecutionUnit ExecutionUnit { get; }

    protected internal TimeSpan? Timeout { get; internal set; }

    protected internal TimeSpan? RetryInterval { get; internal set; }

    TimeSpan? IVerificationProvider<TOwner>.Timeout
    {
        get => Timeout;
        set => Timeout = value;
    }

    TimeSpan? IVerificationProvider<TOwner>.RetryInterval
    {
        get => RetryInterval;
        set => RetryInterval = value;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TVerificationProvider WithRetry
    {
        get
        {
            Timeout = Strategy.GetDefaultTimeout(ExecutionUnit);
            RetryInterval = Strategy.GetDefaultRetryInterval(ExecutionUnit);

            return (TVerificationProvider)this;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TVerificationProvider AtOnce
    {
        get
        {
            Timeout = TimeSpan.Zero;

            return (TVerificationProvider)this;
        }
    }

    /// <summary>
    /// Gets the same instance of <typeparamref name="TVerificationProvider"/>
    /// with <see cref="StringComparer.OrdinalIgnoreCase"/> equality comparer added to its <see cref="TypeEqualityComparerMap"/>.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TVerificationProvider IgnoringCase =>
        Using(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc cref="IVerificationProvider{TOwner}.TypeEqualityComparerMap"/>
    protected Dictionary<Type, object> TypeEqualityComparerMap { get; set; }

    Dictionary<Type, object> IVerificationProvider<TOwner>.TypeEqualityComparerMap
    {
        get => TypeEqualityComparerMap;
        set => TypeEqualityComparerMap = value;
    }

    public TVerificationProvider Using<TVerificationStrategy>()
        where TVerificationStrategy : IVerificationStrategy, new() =>
        Using(new TVerificationStrategy());

    public TVerificationProvider Using(IVerificationStrategy strategy)
    {
        Strategy = strategy.CheckNotNull(nameof(strategy));

        return (TVerificationProvider)this;
    }

    /// <summary>
    /// Adds the specified equality comparer to the <see cref="TypeEqualityComparerMap"/>.
    /// </summary>
    /// <typeparam name="T">The type of object.</typeparam>
    /// <param name="equalityComparer">The equality comparer.</param>
    /// <returns>The same instance of <typeparamref name="TVerificationProvider"/>.</returns>
    public TVerificationProvider Using<T>(IEqualityComparer<T> equalityComparer)
    {
        TypeEqualityComparerMap ??= new Dictionary<Type, object>(1);

        TypeEqualityComparerMap[typeof(T)] = equalityComparer;

        return (TVerificationProvider)this;
    }

    public TVerificationProvider Within(TimeSpan timeout, TimeSpan? retryInterval = null)
    {
        Timeout = timeout;
        RetryInterval = retryInterval ?? RetryInterval;

        return (TVerificationProvider)this;
    }

    public TVerificationProvider WithinSeconds(double timeoutSeconds, double? retryIntervalSeconds = null) =>
        Within(TimeSpan.FromSeconds(timeoutSeconds), retryIntervalSeconds.HasValue ? TimeSpan.FromSeconds(retryIntervalSeconds.Value) : null);

    public TVerificationProvider WithRetryInterval(TimeSpan retryInterval)
    {
        RetryInterval = retryInterval;

        return (TVerificationProvider)this;
    }

    public TVerificationProvider WithRetryIntervalSeconds(double retryIntervalSeconds) =>
        WithRetryInterval(TimeSpan.FromSeconds(retryIntervalSeconds));

    IEqualityComparer<T> IVerificationProvider<TOwner>.ResolveEqualityComparer<T>() =>
        TypeEqualityComparerMap != null && TypeEqualityComparerMap.TryGetValue(typeof(T), out var equalityComparer)
            ? equalityComparer as IEqualityComparer<T>
            : EqualityComparer<T>.Default;

    StringComparison IVerificationProvider<TOwner>.ResolveStringComparison() =>
        ResolveStringComparison();

    protected StringComparison ResolveStringComparison() =>
        TypeEqualityComparerMap != null && TypeEqualityComparerMap.TryGetValue(typeof(string), out var equalityComparer)
            ? Convert((IEqualityComparer<string>)equalityComparer)
            : DefaultStringComparison;

    private static StringComparison Convert(IEqualityComparer<string> equalityComparer)
    {
        if (equalityComparer == StringComparer.CurrentCulture)
            return StringComparison.CurrentCulture;
        else if (equalityComparer == StringComparer.CurrentCultureIgnoreCase)
            return StringComparison.CurrentCultureIgnoreCase;
        if (equalityComparer == StringComparer.InvariantCulture)
            return StringComparison.InvariantCulture;
        else if (equalityComparer == StringComparer.InvariantCultureIgnoreCase)
            return StringComparison.InvariantCultureIgnoreCase;
        else if (equalityComparer == StringComparer.Ordinal)
            return StringComparison.Ordinal;
        else if (equalityComparer == StringComparer.OrdinalIgnoreCase)
            return StringComparison.OrdinalIgnoreCase;
        else
            throw new ArgumentException(
                $"Cannot resolve {nameof(StringComparison)} by the specified '{nameof(equalityComparer)}'. Unknown IEqualityComparer<string> instance.", nameof(equalityComparer));
    }

    (TimeSpan Timeout, TimeSpan RetryInterval) IVerificationProvider<TOwner>.GetRetryOptions() =>
        GetRetryOptions();

    protected virtual (TimeSpan Timeout, TimeSpan RetryInterval) GetRetryOptions() =>
        (Timeout: Timeout ?? Strategy.GetDefaultTimeout(ExecutionUnit),
        RetryInterval: RetryInterval ?? Strategy.GetDefaultRetryInterval(ExecutionUnit));
}
