namespace Atata;

/// <summary>
/// Represents the builder of <see cref="AtataContext"/>.
/// </summary>
public sealed class AtataContextBuilder : ICloneable
{
    private TimeSpan? _waitingTimeout;

    private TimeSpan? _waitingRetryInterval;

    private TimeSpan? _verificationTimeout;

    private TimeSpan? _verificationRetryInterval;

    internal AtataContextBuilder(AtataContextScope? contextScope)
        : this(contextScope, ResolveSessionDefaultStartScopes(contextScope))
    {
    }

    internal AtataContextBuilder(AtataContextScope? contextScope, AtataContextScopes? sessionStartScopes)
    {
        Scope = contextScope;
        Sessions = new(this, [], sessionStartScopes);
        Attributes = new(this, new());
        EventSubscriptions = new(this);
        LogConsumers = new(this);
    }

    /// <summary>
    /// Gets the parent <see cref="AtataContext"/>.
    /// When it is <see langword="null"/>, will try to find a parent context automatically
    /// searching in <see cref="AtataContext.Global"/> descendant contexts considering this
    /// builder's <see cref="Scope"/>, <see cref="TestNameFactory"/>,
    /// <see cref="TestSuiteTypeFactory"/>, <see cref="TestSuiteNameFactory"/>
    /// and <see cref="TestSuiteGroupNameFactory"/>.
    /// </summary>
    public AtataContext? ParentContext { get; private set; }

    /// <summary>
    /// Gets the scope of context.
    /// </summary>
    public AtataContextScope? Scope { get; private set; }

    public AtataSessionsBuilder Sessions { get; private set; }

    /// <summary>
    /// Gets the builder of context attributes,
    /// which provides the functionality to add extra attributes to different metadata levels:
    /// global, assembly, component and property.
    /// </summary>
    public AttributesBuilder Attributes { get; private set; }

    /// <summary>
    /// Gets the builder of event subscriptions,
    /// which provides the methods to subscribe to Atata and custom events.
    /// </summary>
    public AtataContextEventSubscriptionsBuilder EventSubscriptions { get; private set; }

    /// <summary>
    /// Gets the builder of log consumers,
    /// which provides the methods to add log consumers.
    /// </summary>
    public LogConsumersBuilder LogConsumers { get; private set; }

    /// <summary>
    /// Gets the variables dictionary.
    /// </summary>
    public IDictionary<string, object?> Variables { get; private set; } = new Dictionary<string, object?>();

    /// <summary>
    /// Gets the state dictionary.
    /// </summary>
    public IDictionary<string, object?> State { get; private set; } = new Dictionary<string, object?>();

    /// <summary>
    /// Gets the list of secret strings to mask in log.
    /// </summary>
    public List<SecretStringToMask> SecretStringsToMaskInLog { get; private set; } = [];

    /// <summary>
    /// Gets or sets the factory method of the test name.
    /// </summary>
    public Func<string?>? TestNameFactory { get; set; }

    /// <summary>
    /// Gets or sets the factory method of the test suite name.
    /// </summary>
    public Func<string?>? TestSuiteNameFactory { get; set; }

    /// <summary>
    /// Gets or sets the factory method of the test suite type.
    /// </summary>
    public Func<Type?>? TestSuiteTypeFactory { get; set; }

    /// <summary>
    /// Gets or sets the factory method of the test suite group name.
    /// </summary>
    public Func<string?>? TestSuiteGroupNameFactory { get; set; }

    /// <summary>
    /// Gets or sets the factory method of the test suite group name.
    /// </summary>
    public Func<IReadOnlyList<TestTrait>?>? TestTraitsFactory { get; set; }

    /// <summary>
    /// Gets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan BaseRetryTimeout { get; internal set; } = AtataContext.DefaultRetryTimeout;

    /// <summary>
    /// Gets the base retry interval.
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    public TimeSpan BaseRetryInterval { get; internal set; } = AtataContext.DefaultRetryInterval;

    /// <summary>
    /// Gets the waiting timeout.
    /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to 5 seconds by default.
    /// </summary>
    public TimeSpan WaitingTimeout
    {
        get => _waitingTimeout ?? BaseRetryTimeout;
        internal set => _waitingTimeout = value;
    }

    /// <summary>
    /// Gets the waiting retry interval.
    /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>200</c> milliseconds by default.
    /// </summary>
    public TimeSpan WaitingRetryInterval
    {
        get => _waitingRetryInterval ?? BaseRetryInterval;
        internal set => _waitingRetryInterval = value;
    }

    /// <summary>
    /// Gets the verification timeout.
    /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to <c>5</c> seconds by default.
    /// </summary>
    public TimeSpan VerificationTimeout
    {
        get => _verificationTimeout ?? BaseRetryTimeout;
        internal set => _verificationTimeout = value;
    }

    /// <summary>
    /// Gets the verification retry interval.
    /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>200</c> milliseconds by default.
    /// </summary>
    public TimeSpan VerificationRetryInterval
    {
        get => _verificationRetryInterval ?? BaseRetryInterval;
        internal set => _verificationRetryInterval = value;
    }

    /// <summary>
    /// Gets or sets the default cancellation token.
    /// The default value is <see cref="CancellationToken.None"/>.
    /// </summary>
    public CancellationToken DefaultCancellationToken { get; set; }

    /// <summary>
    /// Gets or sets the culture.
    /// </summary>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Gets or sets the assertion exception factory.
    /// The default value is an instance of <see cref="AtataAssertionExceptionFactory"/>.
    /// </summary>
    public IAssertionExceptionFactory AssertionExceptionFactory { get; set; } = AtataAssertionExceptionFactory.Instance;

    /// <summary>
    /// Gets or sets the aggregate assertion exception factory.
    /// The default value is an instance of <see cref="AtataAggregateAssertionExceptionFactory"/>.
    /// </summary>
    public IAggregateAssertionExceptionFactory AggregateAssertionExceptionFactory { get; set; } = AtataAggregateAssertionExceptionFactory.Instance;

    /// <summary>
    /// Gets or sets the aggregate assertion strategy.
    /// The default value is an instance of <see cref="AtataAggregateAssertionStrategy"/>.
    /// </summary>
    public IAggregateAssertionStrategy AggregateAssertionStrategy { get; set; } = AtataAggregateAssertionStrategy.Instance;

    /// <summary>
    /// Gets or sets the strategy for warning assertion reporting.
    /// The default value is an instance of <see cref="AtataWarningReportStrategy"/>.
    /// </summary>
    public IWarningReportStrategy WarningReportStrategy { get; set; } = AtataWarningReportStrategy.Instance;

    /// <summary>
    /// Gets or sets the strategy for assertion failure reporting.
    /// The default value is an instance of <see cref="AtataAssertionFailureReportStrategy"/>.
    /// </summary>
    public IAssertionFailureReportStrategy AssertionFailureReportStrategy { get; set; } = AtataAssertionFailureReportStrategy.Instance;

    /// <summary>
    /// Sets the parent context.
    /// </summary>
    /// <param name="parentContext">The parent context.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseParentContext(AtataContext? parentContext)
    {
        if (Scope == AtataContextScope.Global)
            throw new InvalidOperationException($"Cannot set parent context for global {nameof(AtataContext)}.");

        ParentContext = parentContext;
        return this;
    }

    [Obsolete("Use UseVariable instead.")] // Obsolete since v4.0.0.
    public AtataContextBuilder AddVariable(string key, object? value) =>
        UseVariable(key, value);

    [Obsolete("Use UseVariables instead.")] // Obsolete since v4.0.0.
    public AtataContextBuilder AddVariables(IDictionary<string, object?> variables) =>
        UseVariables(variables);

    /// <summary>
    /// Sets the variable.
    /// </summary>
    /// <param name="key">The variable key.</param>
    /// <param name="value">The variable value.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseVariable(string key, object? value)
    {
        Guard.ThrowIfNullOrWhitespace(key);

        Variables[key] = value;

        return this;
    }

    /// <summary>
    /// Sets the variables.
    /// </summary>
    /// <param name="variables">The variables to set.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseVariables(IEnumerable<KeyValuePair<string, object?>> variables)
    {
        Guard.ThrowIfNull(variables);

        foreach (var variable in variables)
            Variables[variable.Key] = variable.Value;

        return this;
    }

    /// <summary>
    /// Sets the state object.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The state value.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseState<TValue>(TValue value)
    {
        State[StateHierarchicalDictionary.ResolveTypeKey<TValue>()] = value;

        return this;
    }

    /// <summary>
    /// Sets the state object.
    /// </summary>
    /// <param name="key">The state key.</param>
    /// <param name="value">The state value.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseState(string key, object value)
    {
        Guard.ThrowIfNullOrWhitespace(key);

        State[key] = value;

        return this;
    }

    /// <summary>
    /// Sets the state objects.
    /// </summary>
    /// <param name="objects">The objects to set.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseState(IEnumerable<KeyValuePair<string, object>> objects)
    {
        Guard.ThrowIfNull(objects);

        foreach (var variable in objects)
            State[variable.Key] = variable.Value;

        return this;
    }

    /// <summary>
    /// Adds the secret string to mask in log.
    /// </summary>
    /// <param name="value">The secret string value.</param>
    /// <param name="mask">The mask, which should replace the secret string.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder AddSecretStringToMaskInLog(string value, string mask = "{*****}")
    {
        SecretStringsToMaskInLog.Add(new(value, mask));

        return this;
    }

    /// <summary>
    /// Sets the factory method of the test name.
    /// </summary>
    /// <param name="testNameFactory">The factory method of the test name.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestName(Func<string?> testNameFactory)
    {
        Guard.ThrowIfNull(testNameFactory);

        TestNameFactory = testNameFactory;
        return this;
    }

    /// <summary>
    /// Sets the name of the test.
    /// </summary>
    /// <param name="testName">The name of the test.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestName(string? testName)
    {
        TestNameFactory = () => testName;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test suite (class) name.
    /// </summary>
    /// <param name="testSuiteNameFactory">The factory method of the test suite name.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteName(Func<string?> testSuiteNameFactory)
    {
        Guard.ThrowIfNull(testSuiteNameFactory);

        TestSuiteNameFactory = testSuiteNameFactory;
        return this;
    }

    /// <summary>
    /// Sets the name of the test suite (class).
    /// </summary>
    /// <param name="testSuiteName">The name of the test suite.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteName(string? testSuiteName)
    {
        TestSuiteNameFactory = () => testSuiteName;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test suite (fixture/class) type.
    /// </summary>
    /// <param name="testSuiteTypeFactory">The factory method of the test suite type.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteType(Func<Type?> testSuiteTypeFactory)
    {
        Guard.ThrowIfNull(testSuiteTypeFactory);

        TestSuiteTypeFactory = testSuiteTypeFactory;
        return this;
    }

    /// <summary>
    /// Sets the type of the test suite (fixture/class).
    /// </summary>
    /// <param name="testSuiteType">The type of the test suite.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteType(Type? testSuiteType)
    {
        TestSuiteTypeFactory = () => testSuiteType;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test suite group (collection fixture) name.
    /// </summary>
    /// <param name="testSuiteGroupNameFactory">The factory method of the test suite group name.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteGroupName(Func<string?> testSuiteGroupNameFactory)
    {
        Guard.ThrowIfNull(testSuiteGroupNameFactory);

        TestSuiteGroupNameFactory = testSuiteGroupNameFactory;
        return this;
    }

    /// <summary>
    /// Sets the name of the test suite group (collection fixture).
    /// </summary>
    /// <param name="testSuiteGroupName">The name of the test suite group.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteGroupName(string? testSuiteGroupName)
    {
        TestSuiteGroupNameFactory = () => testSuiteGroupName;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test traits.
    /// </summary>
    /// <param name="testTraitsFactory">The factory method of the test traits.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestTraits(Func<IReadOnlyList<TestTrait>?> testTraitsFactory)
    {
        Guard.ThrowIfNull(testTraitsFactory);

        TestTraitsFactory = testTraitsFactory;
        return this;
    }

    /// <summary>
    /// Sets the test traits.
    /// </summary>
    /// <param name="testTraits">The test traits.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestTraits(IReadOnlyList<TestTrait>? testTraits)
    {
        TestTraitsFactory = () => testTraits;
        return this;
    }

    /// <summary>
    /// Sets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseBaseRetryTimeout(TimeSpan timeout)
    {
        BaseRetryTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Sets the base retry interval.
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseBaseRetryInterval(TimeSpan interval)
    {
        BaseRetryInterval = interval;
        return this;
    }

    /// <summary>
    /// Sets the waiting timeout.
    /// The default value is taken from <see cref="BaseRetryTimeout"/>,
    /// which is equal to <c>5</c> seconds by default.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseWaitingTimeout(TimeSpan timeout)
    {
        WaitingTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Sets the waiting retry interval.
    /// The default value is taken from <see cref="BaseRetryInterval"/>,
    /// which is equal to <c>200</c> milliseconds by default.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseWaitingRetryInterval(TimeSpan interval)
    {
        WaitingRetryInterval = interval;
        return this;
    }

    /// <summary>
    /// Sets the verification timeout.
    /// The default value is taken from <see cref="BaseRetryTimeout"/>,
    /// which is equal to <c>5</c> seconds by default.
    /// </summary>
    /// <param name="timeout">The retry timeout.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseVerificationTimeout(TimeSpan timeout)
    {
        VerificationTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Sets the verification retry interval.
    /// The default value is taken from <see cref="BaseRetryInterval"/>,
    /// which is equal to <c>200</c> milliseconds by default.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseVerificationRetryInterval(TimeSpan interval)
    {
        VerificationRetryInterval = interval;
        return this;
    }

    /// <summary>
    /// Sets the default cancellation token.
    /// The default value is <see cref="CancellationToken.None"/>.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseDefaultCancellationToken(CancellationToken cancellationToken)
    {
        DefaultCancellationToken = cancellationToken;
        return this;
    }

    /// <summary>
    /// Sets the culture.
    /// The default value is <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    /// <param name="culture">The culture.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseCulture(CultureInfo culture)
    {
        Culture = culture;
        return this;
    }

    /// <summary>
    /// Sets the culture by the name.
    /// The default value is <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    /// <param name="cultureName">The name of the culture.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseCulture(string cultureName) =>
        UseCulture(CultureInfo.GetCultureInfo(cultureName));

    [Obsolete("Use UseAssertionExceptionFactory(...) instead for custom exception, " +
        "or use features of one of the libraries: Atata.NUnit, Atata.Xunit, Atata.MSTest, etc.")] // Obsolete since v4.0.0.
    public AtataContextBuilder UseAssertionExceptionType<TException>()
        where TException : Exception
        =>
        UseAssertionExceptionType(typeof(TException));

    [Obsolete("Use UseAssertionExceptionFactory(...) instead for custom exception, " +
        "or use features of one of the libraries: Atata.NUnit, Atata.Xunit, Atata.MSTest, etc.")] // Obsolete since v4.0.0.
    public AtataContextBuilder UseAssertionExceptionType(Type exceptionType)
    {
        Guard.ThrowIfNot<Exception>(exceptionType);

        return UseAssertionExceptionFactory(new TypeBasedAssertionExceptionFactory(exceptionType));
    }

    /// <summary>
    /// Sets the assertion exception factory.
    /// The default value is an instance of <see cref="AtataAssertionExceptionFactory"/>.
    /// </summary>
    /// <param name="factory">The assertion exception factory.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssertionExceptionFactory(IAssertionExceptionFactory factory)
    {
        AssertionExceptionFactory = factory;
        return this;
    }

    [Obsolete("Use UseAggregateAssertionExceptionFactory(...) instead for custom exception, " +
        "or use features of one of the libraries: Atata.NUnit, Atata.Xunit, Atata.MSTest, etc.")] // Obsolete since v4.0.0.
    public AtataContextBuilder UseAggregateAssertionExceptionType<TException>()
        where TException : Exception
        =>
        UseAggregateAssertionExceptionType(typeof(TException));

    [Obsolete("Use UseAggregateAssertionExceptionFactory(...) instead for custom exception, " +
        "or use features of one of the libraries: Atata.NUnit, Atata.Xunit, Atata.MSTest, etc.")] // Obsolete since v4.0.0.
    public AtataContextBuilder UseAggregateAssertionExceptionType(Type exceptionType)
    {
        Guard.ThrowIfNot<Exception>(exceptionType);

        return UseAggregateAssertionExceptionFactory(new TypeBasedAggregateAssertionExceptionFactory(exceptionType));
    }

    /// <summary>
    /// Sets the aggregate assertion exception factory.
    /// The default value is an instance of <see cref="AtataAggregateAssertionExceptionFactory"/>.
    /// </summary>
    /// <param name="factory">The aggregate assertion exception factory.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionExceptionFactory(IAggregateAssertionExceptionFactory factory)
    {
        AggregateAssertionExceptionFactory = factory;
        return this;
    }

    [Obsolete("Use UseAggregateAssertionStrategy(IAggregateAssertionStrategy) instead.")] // Obsolete since v4.0.0.
    public AtataContextBuilder UseAggregateAssertionStrategy<TAggregateAssertionStrategy>()
        where TAggregateAssertionStrategy : IAggregateAssertionStrategy, new()
    {
        TAggregateAssertionStrategy strategy = new();

        return UseAggregateAssertionStrategy(strategy);
    }

    /// <summary>
    /// Sets the aggregate assertion strategy.
    /// </summary>
    /// <param name="strategy">The aggregate assertion strategy.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionStrategy(IAggregateAssertionStrategy strategy)
    {
        AggregateAssertionStrategy = strategy;

        return this;
    }

    /// <summary>
    /// Sets the strategy for warning assertion reporting.
    /// </summary>
    /// <param name="strategy">The warning report strategy.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseWarningReportStrategy(IWarningReportStrategy strategy)
    {
        WarningReportStrategy = strategy;

        return this;
    }

    /// <summary>
    /// Sets the strategy for assertion failure reporting.
    /// </summary>
    /// <param name="strategy">The assertion failure reporting strategy.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssertionFailureReportStrategy(IAssertionFailureReportStrategy strategy)
    {
        AssertionFailureReportStrategy = strategy;

        return this;
    }

    /// <summary>
    /// Creates a new clear <see cref="AtataContextBuilder"/> with the same scope arguments.
    /// If this instance is <see cref="AtataContext.BaseConfiguration"/>,
    /// sets the new cleared instance into <see cref="AtataContext.BaseConfiguration"/>.
    /// </summary>
    /// <returns>The new cleared <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Clear()
    {
        AtataContextBuilder newBuilder = new(Scope, Sessions.DefaultStartScopes);

        if (AtataContext.BaseConfiguration == this)
            AtataContext.BaseConfiguration = newBuilder;

        return newBuilder;
    }

    /// <inheritdoc cref="BuildAsync(CancellationToken)"/>
    public AtataContext Build(CancellationToken cancellationToken = default)
    {
        AtataContext context = CreateAtataContext();

        InitializeContextAsync(context, cancellationToken).RunSync();

        return context;
    }

    /// <summary>
    /// Builds the <see cref="AtataContext" /> instance and sets it to <see cref="AtataContext.Current" /> property.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created <see cref="AtataContext"/> instance.</returns>
    public async Task<AtataContext> BuildAsync(CancellationToken cancellationToken = default)
    {
        AtataContext context = CreateAtataContext();

        await InitializeContextAsync(context, cancellationToken).ConfigureAwait(false);

        return context;
    }

    private AtataContext CreateAtataContext()
    {
        string? testName = Scope is null or AtataContextScope.Test
            ? TestNameFactory?.Invoke()
            : null;

        TestInfo testInfo = new(
            testName,
            TestSuiteTypeFactory?.Invoke(),
            TestSuiteNameFactory?.Invoke(),
            TestSuiteGroupNameFactory?.Invoke(),
            TestTraitsFactory?.Invoke());

        AtataContext? parentContext = ParentContext;

        if (parentContext is null && Scope is not null && AtataContext.Global is not null)
            parentContext = AtataContextParentResolver.FindParentContext(AtataContext.Global, Scope.Value, testInfo);

        AtataContext context = new(parentContext, Scope, testInfo)
        {
            Attributes = Attributes.AttributesContext.Clone(),
            BaseRetryTimeout = BaseRetryTimeout,
            BaseRetryInterval = BaseRetryInterval,
            WaitingTimeout = WaitingTimeout,
            WaitingRetryInterval = WaitingRetryInterval,
            VerificationTimeout = VerificationTimeout,
            VerificationRetryInterval = VerificationRetryInterval,
            DefaultCancellationToken = DefaultCancellationToken,
            Culture = Culture ?? CultureInfo.CurrentCulture,
            AssertionExceptionFactory = AssertionExceptionFactory,
            AggregateAssertionExceptionFactory = AggregateAssertionExceptionFactory,
            AggregateAssertionStrategy = AggregateAssertionStrategy ?? AtataAggregateAssertionStrategy.Instance,
            WarningReportStrategy = WarningReportStrategy ?? AtataWarningReportStrategy.Instance,
            AssertionFailureReportStrategy = AssertionFailureReportStrategy ?? AtataAssertionFailureReportStrategy.Instance
        };

        context.EventBus = new EventBus(context, EventSubscriptions.GetItemsForScope(Scope));

        context.InitMainVariables();
        context.InitCustomVariables(Variables);
        context.InitState(State);
        context.InitArtifactsDirectory();
        context.InitArtifactsVariable();

        context.Log = CreateLogManager(context);

        parentContext?.AddChildContext(context);

        if (Scope == AtataContextScope.Global)
            AtataContext.Global = context;

        AtataContext.Current = context;

        context.EventBus.Publish(new AtataContextPreInitEvent(context));

        return context;
    }

    private LogManager CreateLogManager(AtataContext context)
    {
        LogManagerConfiguration configuration = new(
            [.. LogConsumers.GetItemsForScope(Scope)],
            [.. SecretStringsToMaskInLog]);

        foreach (var item in configuration.ConsumerConfigurations)
        {
            if (item.Consumer is IInitializableLogConsumer initializableLogConsumer)
                initializableLogConsumer.Initialize(context);
        }

        return new(configuration, new AtataContextLogEventInfoFactory(context));
    }

    private async Task InitializeContextAsync(AtataContext context, CancellationToken cancellationToken)
    {
        context.LogTestStart();

        context.SetToDefaultCancellationTokenWhenDefault(ref cancellationToken);

        try
        {
            await context.Log.ExecuteSectionAsync(
                new LogSection($"Initialize {context.ToShortString()}", LogLevel.Trace),
                async () => await DoInitializeContextAsync(context, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            context.Log.Error(exception, "AtataContext initialization failed.");
            await context.DisposeAsync();
            throw;
        }

        context.BodyExecutionStopwatch.Start();
    }

    private async Task DoInitializeContextAsync(AtataContext context, CancellationToken cancellationToken)
    {
        if (Culture != null)
            ApplyCulture(context, Culture);

        context.Log.Trace($"Set: Artifacts={context.ArtifactsPath}");

        await context.EventBus.PublishAsync(new AtataContextInitStartedEvent(context, this), cancellationToken)
            .ConfigureAwait(false);

        foreach (var sessionBuilder in Sessions.Builders)
            sessionBuilder.TargetContext = context;

        context.Sessions.AddBuilders(Sessions.Builders);

        foreach (var provider in Sessions.GetProvidersForScope(Scope))
            await provider.StartAsync(context, cancellationToken)
                .ConfigureAwait(false);

        await context.EventBus.PublishAsync(new AtataContextInitCompletedEvent(context), cancellationToken)
            .ConfigureAwait(false);

        context.Activate();
    }

    private static void ApplyCulture(AtataContext context, CultureInfo culture)
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;

        if (AtataContext.GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.Static)
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = culture;

        context.Log.Trace($"Set: Culture={culture.Name}");
    }

    [Obsolete("Instead use EventSubscriptions.Add(SetUpWebDriversForUseEventHandler.Instance) for global AtataContextBuilder before build. " +
        "Alternatively use EventSubscriptions.Add(new SetUpWebDriversEventHandler(BrowserNames...)) to specify driver names explicitly.")] // Obsolete since v4.0.0.
    public void AutoSetUpDriverToUse() =>
        throw new NotSupportedException();

    [Obsolete("Instead use EventSubscriptions.Add(SetUpWebDriversForUseEventHandler.Instance) for global AtataContextBuilder before build. " +
        "Alternatively use EventSubscriptions.Add(new SetUpWebDriversEventHandler(BrowserNames...)) to specify driver names explicitly.")] // Obsolete since v4.0.0.
    public Task AutoSetUpDriverToUseAsync() =>
        throw new NotSupportedException();

    [Obsolete("Instead use EventSubscriptions.Add(SetUpWebDriversConfiguredEventHandler.Instance) for global AtataContextBuilder before build. " +
        "Alternatively use EventSubscriptions.Add(new SetUpWebDriversEventHandler(BrowserNames...)) to specify driver names explicitly.")] // Obsolete since v4.0.0.
    public void AutoSetUpConfiguredDrivers() =>
        throw new NotSupportedException();

    [Obsolete("Instead use EventSubscriptions.Add(SetUpWebDriversConfiguredEventHandler.Instance) for global AtataContextBuilder before build. " +
        "Alternatively use EventSubscriptions.Add(new SetUpWebDriversEventHandler(BrowserNames...)) to specify driver names explicitly.")] // Obsolete since v4.0.0.
    public Task AutoSetUpConfiguredDriversAsync() =>
        throw new NotSupportedException();

    object ICloneable.Clone() =>
        Clone();

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>The copied <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Clone() =>
        CopyFor(Scope);

    /// <summary>
    /// Creates a copy of the current builder for the specified <paramref name="scope"/>.
    /// </summary>
    /// <param name="scope">The scope of context.</param>
    /// <returns>The copied <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder CloneFor(AtataContextScope scope) =>
        CopyFor(scope);

    internal AtataContextBuilder CopyFor(AtataContextScope? scope)
    {
        var copy = (AtataContextBuilder)MemberwiseClone();

        copy.Scope = scope;

        copy.Sessions = new(
            copy,
            [.. Sessions.Providers.Select(x => (IAtataSessionProvider)x.Clone())],
            ResolveSessionDefaultStartScopes(scope));

        copy.Attributes = new(
            copy,
            Attributes.AttributesContext.Clone());

        copy.EventSubscriptions = EventSubscriptions.CloneFor(copy);
        copy.LogConsumers = LogConsumers.CloneFor(copy);

        copy.Variables = new Dictionary<string, object?>(Variables);
        copy.State = new Dictionary<string, object?>(State);
        copy.SecretStringsToMaskInLog = [.. SecretStringsToMaskInLog];

        return copy;
    }

    private static AtataContextScopes? ResolveSessionDefaultStartScopes(AtataContextScope? scope) =>
        scope switch
        {
            AtataContextScope.Test => AtataContextScopes.Test,
            AtataContextScope.TestSuite => AtataContextScopes.TestSuite,
            AtataContextScope.TestSuiteGroup => AtataContextScopes.TestSuiteGroup,
            AtataContextScope.Namespace => AtataContextScopes.Namespace,
            AtataContextScope.Global => AtataContextScopes.Global,
            null => null,
            _ => AtataContextScopes.None
        };
}
