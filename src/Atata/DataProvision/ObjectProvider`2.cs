namespace Atata;

/// <summary>
/// Represents the base object provider class.
/// </summary>
/// <typeparam name="TObject">The type of the provided object.</typeparam>
/// <typeparam name="TOwner">The type of the owner object.</typeparam>
[DebuggerDisplay("{ProviderName,nq}")]
public abstract class ObjectProvider<TObject, TOwner> :
    IObjectProvider<TObject, TOwner>,
    IHasProviderName,
    IHasSourceProviderName
{
    private readonly IObjectSource<TObject> _objectSource;

    private readonly IAtataExecutionUnit? _executionUnit;

    private string? _sourceProviderName;

    private string _providerName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectProvider{TObject, TOwner}"/> class.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    protected ObjectProvider(
        IObjectSource<TObject> objectSource,
        string providerName,
        IAtataExecutionUnit? executionUnit = null)
    {
        Guard.ThrowIfNull(objectSource);
        Guard.ThrowIfNull(providerName);

        _objectSource = objectSource;
        _providerName = providerName;
        _executionUnit = executionUnit;
    }

    /// <inheritdoc/>
    public string ProviderName
    {
        get
        {
            string? actualSourceProviderName = SourceProviderName;

            return actualSourceProviderName is null or []
                ? _providerName
                : BuildProviderName(actualSourceProviderName, _providerName);
        }
        set => _providerName = value;
    }

    /// <inheritdoc/>
    public string? SourceProviderName
    {
        get => _sourceProviderName ?? _objectSource.SourceProviderName;
        set => _sourceProviderName = value;
    }

    TObject IObjectProvider<TObject>.Object =>
        Object;

    /// <inheritdoc cref="IObjectProvider{TObject}.Object"/>
    protected virtual TObject Object =>
        _objectSource.Object;

    /// <summary>
    /// Gets the owner object.
    /// </summary>
    protected abstract TOwner Owner { get; }

    TOwner IObjectProvider<TObject, TOwner>.Owner => Owner;

    /// <summary>
    /// Gets the assertion verification provider that has a set of verification extension methods.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ObjectVerificationProvider<TObject, TOwner> Should =>
        CreateVerificationProvider();

    /// <summary>
    /// Gets the expectation verification provider that has a set of verification extension methods.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ObjectVerificationProvider<TObject, TOwner> ExpectTo =>
        CreateVerificationProvider().Using(ExpectationVerificationStrategy.Instance);

    /// <summary>
    /// Gets the waiting verification provider that has a set of verification extension methods.
    /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/>
    /// of executing <see cref="AtataContext"/> for timeout and retry interval.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ObjectVerificationProvider<TObject, TOwner> WaitTo =>
        CreateVerificationProvider().Using(WaitingVerificationStrategy.Instance);

    /// <inheritdoc/>
    public bool IsDynamic =>
        _objectSource.IsDynamic;

    /// <summary>
    /// Gets the associated execution unit.
    /// </summary>
    protected IAtataExecutionUnit? ExecutionUnit => _executionUnit;

    IAtataExecutionUnit? IObjectProvider<TObject>.ExecutionUnit => _executionUnit;

    /// <summary>
    /// Performs an implicit conversion from <see cref="ObjectProvider{TObject, TOwner}"/> to <typeparamref name="TObject"/>.
    /// </summary>
    /// <param name="objectProvider">The object provider.</param>
    /// <returns>The value of the <paramref name="objectProvider"/>.</returns>
    public static implicit operator TObject(ObjectProvider<TObject, TOwner> objectProvider) =>
        objectProvider.Object;

    private static string BuildProviderName(string sourceProviderName, string providerName) =>
        providerName[0] == '['
            ? $"{sourceProviderName}{providerName}"
            : $"{sourceProviderName}.{providerName}";

    private ObjectVerificationProvider<TObject, TOwner> CreateVerificationProvider() =>
        new(this, _executionUnit);

    /// <summary>
    /// Resolves the associated or current <see cref="AtataContext"/> instance.
    /// Throws <see cref="AtataContextNotFoundException"/> if an execution unit was not specified
    /// and <see cref="AtataContext.Current"/> is <see langword="null"/>.
    /// </summary>
    /// <returns>An <see cref="AtataContext"/> instance.</returns>
    protected AtataContext ResolveAtataContext() =>
        _executionUnit?.Context ?? AtataContext.ResolveCurrent();

    /// <summary>
    /// Gets the associated or current <see cref="AtataContext"/> instance or <see langword="null"/>.
    /// </summary>
    /// <returns>An <see cref="AtataContext"/> instance.</returns>
    protected AtataContext? GetAtataContextOrNull() =>
        _executionUnit?.Context ?? AtataContext.Current;

    /// <summary>
    /// Tries to get the associated or current <see cref="AtataContext"/> instance.
    /// </summary>
    /// <param name="context">The current context.</param>
    /// <returns><see langword="true"/> if the associated or current context is present; otherwise, <see langword="false"/>.</returns>
    protected bool TryGetAtataContext([NotNullWhen(true)] out AtataContext? context)
    {
        context = GetAtataContextOrNull();
        return context is not null;
    }

    /// <summary>
    /// Tries to get the associated or current context's <see cref="ILogManager"/> instance.
    /// </summary>
    /// <param name="logManager">The current log.</param>
    /// <returns><see langword="true"/> if the associated or current context is present; otherwise, <see langword="false"/>.</returns>
    protected bool TryGetLog([NotNullWhen(true)] out ILogManager? logManager)
    {
        logManager = _executionUnit?.Log ?? AtataContext.Current?.Log;
        return logManager is not null;
    }

    public override string ToString() =>
        ProviderName;
}
