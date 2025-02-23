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

    internal AtataContextBuilder(AtataContextScope? contextScope, AtataSessionStartScopes? sessionStartScopes)
    {
        Scope = contextScope;
        Sessions = new(this, [], sessionStartScopes);
        LogConsumers = new(this, []);
    }

    /// <summary>
    /// Gets the parent <see cref="AtataContext"/>.
    /// When it is <see langword="null"/>, will try to find a parent context automatically
    /// searching in <see cref="AtataContext.Global"/> descendant contexts considering this
    /// builder's <see cref="Scope"/>, <see cref="TestNameFactory"/>,
    /// <see cref="TestSuiteTypeFactory"/>, <see cref="TestSuiteNameFactory"/>
    /// and <see cref="TestSuiteGroupNameFactory"/>.
    /// </summary>
    public AtataContext ParentContext { get; private set; }

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
    public AttributesBuilder Attributes { get; private set; } = new(new());

    /// <summary>
    /// Gets the builder of event subscriptions,
    /// which provides the methods to subscribe to Atata and custom events.
    /// </summary>
    public EventSubscriptionsBuilder EventSubscriptions { get; private set; } = new();

    /// <summary>
    /// Gets the builder of log consumers,
    /// which provides the methods to add log consumers.
    /// </summary>
    public LogConsumersBuilder LogConsumers { get; private set; }

    /// <summary>
    /// Gets the variables dictionary.
    /// </summary>
    public IDictionary<string, object> Variables { get; private set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets the list of secret strings to mask in log.
    /// </summary>
    public List<SecretStringToMask> SecretStringsToMaskInLog { get; private set; } = [];

    /// <summary>
    /// Gets or sets the factory method of the test name.
    /// </summary>
    public Func<string> TestNameFactory { get; set; }

    /// <summary>
    /// Gets or sets the factory method of the test suite name.
    /// </summary>
    public Func<string> TestSuiteNameFactory { get; set; }

    /// <summary>
    /// Gets or sets the factory method of the test suite type.
    /// </summary>
    public Func<Type> TestSuiteTypeFactory { get; set; }

    /// <summary>
    /// Gets or sets the factory method of the test suite group name.
    /// </summary>
    public Func<string> TestSuiteGroupNameFactory { get; set; }

    /// <summary>
    /// Gets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan BaseRetryTimeout { get; internal set; } = AtataContext.DefaultRetryTimeout;

    /// <summary>
    /// Gets the base retry interval.
    /// The default value is <c>500</c> milliseconds.
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
    /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
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
    /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
    /// </summary>
    public TimeSpan VerificationRetryInterval
    {
        get => _verificationRetryInterval ?? BaseRetryInterval;
        internal set => _verificationRetryInterval = value;
    }

    /// <summary>
    /// Gets or sets the culture.
    /// </summary>
    public CultureInfo Culture { get; set; }

    /// <summary>
    /// Gets or sets the type of the assertion exception.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    public Type AssertionExceptionType { get; set; } = typeof(AssertionException);

    /// <summary>
    /// Gets or sets the type of the aggregate assertion exception.
    /// The default value is a type of <see cref="AggregateAssertionException"/>.
    /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
    /// </summary>
    public Type AggregateAssertionExceptionType { get; set; } = typeof(AggregateAssertionException);

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
    public AtataContextBuilder UseParentContext(AtataContext parentContext)
    {
        if (Scope == AtataContextScope.Global)
            throw new InvalidOperationException($"Cannot set parent context for global {nameof(AtataContext)}.");

        ParentContext = parentContext;
        return this;
    }

    [Obsolete("Use SetVariable instead.")] // Obsolete since v4.0.0.
    public AtataContextBuilder AddVariable(string key, object value) =>
        SetVariable(key, value);

    [Obsolete("Use SetVariables instead.")] // Obsolete since v4.0.0.
    public AtataContextBuilder AddVariables(IDictionary<string, object> variables) =>
        SetVariables(variables);

    /// <summary>
    /// Sets the variable.
    /// </summary>
    /// <param name="key">The variable key.</param>
    /// <param name="value">The variable value.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder SetVariable(string key, object value)
    {
        key.CheckNotNullOrWhitespace(nameof(key));

        Variables[key] = value;

        return this;
    }

    /// <summary>
    /// Sets the variables.
    /// </summary>
    /// <param name="variables">The variables to add.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder SetVariables(IDictionary<string, object> variables)
    {
        variables.CheckNotNull(nameof(variables));

        foreach (var variable in variables)
            Variables[variable.Key] = variable.Value;

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
    public AtataContextBuilder UseTestName(Func<string> testNameFactory)
    {
        testNameFactory.CheckNotNull(nameof(testNameFactory));

        TestNameFactory = testNameFactory;
        return this;
    }

    /// <summary>
    /// Sets the name of the test.
    /// </summary>
    /// <param name="testName">The name of the test.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestName(string testName)
    {
        TestNameFactory = () => testName;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test suite (class) name.
    /// </summary>
    /// <param name="testSuiteNameFactory">The factory method of the test suite name.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteName(Func<string> testSuiteNameFactory)
    {
        testSuiteNameFactory.CheckNotNull(nameof(testSuiteNameFactory));

        TestSuiteNameFactory = testSuiteNameFactory;
        return this;
    }

    /// <summary>
    /// Sets the name of the test suite (class).
    /// </summary>
    /// <param name="testSuiteName">The name of the test suite.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteName(string testSuiteName)
    {
        TestSuiteNameFactory = () => testSuiteName;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test suite (fixture/class) type.
    /// </summary>
    /// <param name="testSuiteTypeFactory">The factory method of the test suite type.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteType(Func<Type> testSuiteTypeFactory)
    {
        testSuiteTypeFactory.CheckNotNull(nameof(testSuiteTypeFactory));

        TestSuiteTypeFactory = testSuiteTypeFactory;
        return this;
    }

    /// <summary>
    /// Sets the type of the test suite (fixture/class).
    /// </summary>
    /// <param name="testSuiteType">The type of the test suite.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteType(Type testSuiteType)
    {
        testSuiteType.CheckNotNull(nameof(testSuiteType));

        TestSuiteTypeFactory = () => testSuiteType;
        return this;
    }

    /// <summary>
    /// Sets the factory method of the test suite group (collection fixture) name.
    /// </summary>
    /// <param name="testSuiteGroupNameFactory">The factory method of the test suite group name.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteGroupName(Func<string> testSuiteGroupNameFactory)
    {
        testSuiteGroupNameFactory.CheckNotNull(nameof(testSuiteGroupNameFactory));

        TestSuiteGroupNameFactory = testSuiteGroupNameFactory;
        return this;
    }

    /// <summary>
    /// Sets the name of the test suite group (collection fixture).
    /// </summary>
    /// <param name="testSuiteGroupName">The name of the test suite group.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseTestSuiteGroupName(string testSuiteGroupName)
    {
        TestSuiteGroupNameFactory = () => testSuiteGroupName;
        return this;
    }

    [Obsolete("Use AtataContext.GlobalProperties.UseUtcTimeZone() method instead.")] // Obsolete since v3.0.0.
    public AtataContextBuilder UseUtcTimeZone() =>
        UseTimeZone(TimeZoneInfo.Utc);

    [Obsolete("Use AtataContext.GlobalProperties.UseTimeZone(...) method instead.")] // Obsolete since v3.0.0.
    public AtataContextBuilder UseTimeZone(string timeZoneId)
    {
        timeZoneId.CheckNotNullOrWhitespace(nameof(timeZoneId));
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return UseTimeZone(timeZone);
    }

    [Obsolete("Use AtataContext.GlobalProperties.UseTimeZone(...) method instead.")] // Obsolete since v3.0.0.
    public AtataContextBuilder UseTimeZone(TimeZoneInfo timeZone)
    {
        timeZone.CheckNotNull(nameof(timeZone));

        AtataContext.GlobalProperties.UseTimeZone(timeZone);
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
    /// The default value is <c>500</c> milliseconds.
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
    /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to <c>5</c> seconds by default.
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
    /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
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
    /// The default value is taken from <see cref="BaseRetryTimeout"/>, which is equal to <c>5</c> seconds by default.
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
    /// The default value is taken from <see cref="BaseRetryInterval"/>, which is equal to <c>500</c> milliseconds by default.
    /// </summary>
    /// <param name="interval">The retry interval.</param>
    /// <returns>The same <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseVerificationRetryInterval(TimeSpan interval)
    {
        VerificationRetryInterval = interval;
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

    /// <summary>
    /// Sets the type of the assertion exception.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssertionExceptionType<TException>()
        where TException : Exception
        =>
        UseAssertionExceptionType(typeof(TException));

    /// <summary>
    /// Sets the type of the assertion exception.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    /// <param name="exceptionType">The type of the exception.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAssertionExceptionType(Type exceptionType)
    {
        exceptionType.CheckIs<Exception>(nameof(exceptionType));

        AssertionExceptionType = exceptionType;
        return this;
    }

    /// <summary>
    /// Sets the type of aggregate assertion exception.
    /// The default value is a type of <see cref="AggregateAssertionException"/>.
    /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionExceptionType<TException>()
        where TException : Exception
        =>
        UseAggregateAssertionExceptionType(typeof(TException));

    /// <summary>
    /// Sets the type of aggregate assertion exception.
    /// The default value is a type of <see cref="AggregateAssertionException"/>.
    /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
    /// </summary>
    /// <param name="exceptionType">The type of the exception.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionExceptionType(Type exceptionType)
    {
        exceptionType.CheckIs<Exception>(nameof(exceptionType));

        AggregateAssertionExceptionType = exceptionType;
        return this;
    }

    /// <summary>
    /// Defines that the name of the test should be taken from the NUnit test.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitTestName() =>
        UseTestName(NUnitAdapter.GetCurrentTestName);

    /// <summary>
    /// Defines that the name of the test suite should be taken from the NUnit test fixture.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitTestSuiteName() =>
        UseTestSuiteName(NUnitAdapter.GetCurrentTestFixtureName);

    /// <summary>
    /// Defines that the type of the test suite should be taken from the NUnit test fixture.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitTestSuiteType() =>
        UseTestSuiteType(NUnitAdapter.GetCurrentTestFixtureType);

    /// <summary>
    /// Sets <see cref="NUnitAggregateAssertionStrategy"/> as the aggregate assertion strategy.
    /// The <see cref="NUnitAggregateAssertionStrategy"/> uses NUnit's <c>Assert.Multiple</c> method for aggregate assertion.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitAggregateAssertionStrategy() =>
        UseAggregateAssertionStrategy(NUnitAggregateAssertionStrategy.Instance);

    /// <summary>
    /// Sets the aggregate assertion strategy.
    /// </summary>
    /// <typeparam name="TAggregateAssertionStrategy">The type of the aggregate assertion strategy.</typeparam>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAggregateAssertionStrategy<TAggregateAssertionStrategy>()
        where TAggregateAssertionStrategy : IAggregateAssertionStrategy, new()
    {
        IAggregateAssertionStrategy strategy = Activator.CreateInstance<TAggregateAssertionStrategy>();

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
    /// Sets <see cref="NUnitWarningReportStrategy"/> as the strategy for warning assertion reporting.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitWarningReportStrategy() =>
        UseWarningReportStrategy(NUnitWarningReportStrategy.Instance);

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
    /// Uses the <see cref="NUnitAssertionFailureReportStrategy"/> as the strategy for assertion failure reporting.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitAssertionFailureReportStrategy() =>
        UseAssertionFailureReportStrategy(NUnitAssertionFailureReportStrategy.Instance);

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
    /// Sets the type of <c>NUnit.Framework.AssertionException</c> as the assertion exception type.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseNUnitAssertionExceptionType() =>
        UseAssertionExceptionType(NUnitAdapter.AssertionExceptionType);

    /// <summary>
    /// Enables all Atata features for NUnit.
    /// Executes the following methods:
    /// <list type="bullet">
    /// <item><see cref="UseNUnitTestName"/></item>
    /// <item><see cref="UseNUnitTestSuiteName"/></item>
    /// <item><see cref="UseNUnitTestSuiteType"/></item>
    /// <item><see cref="UseNUnitAssertionExceptionType"/></item>
    /// <item><see cref="UseNUnitAggregateAssertionStrategy"/></item>
    /// <item><see cref="UseNUnitWarningReportStrategy"/></item>
    /// <item><see cref="UseNUnitAssertionFailureReportStrategy"/></item>
    /// <item><see cref="NUnitLogConsumersBuilderExtensions.AddNUnitTestContext(LogConsumersBuilder, Action{LogConsumerBuilder{NUnitTestContextLogConsumer}}?)"/> for <see cref="LogConsumers"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.LogNUnitError(EventSubscriptionsBuilder)"/> for <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.TakeScreenshotOnNUnitError(EventSubscriptionsBuilder, string)"/> for <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.TakePageSnapshotOnNUnitError(EventSubscriptionsBuilder, string)"/> for <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.AddArtifactsToNUnitTestContext(EventSubscriptionsBuilder)"/> for <see cref="EventSubscriptions"/> property</item>
    /// </list>
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseAllNUnitFeatures()
    {
        UseNUnitTestName();
        UseNUnitTestSuiteName();
        UseNUnitTestSuiteType();
        UseNUnitAssertionExceptionType();
        UseNUnitAggregateAssertionStrategy();
        UseNUnitWarningReportStrategy();
        UseNUnitAssertionFailureReportStrategy();
        LogConsumers.AddNUnitTestContext();
        EventSubscriptions.LogNUnitError();
        EventSubscriptions.TakeScreenshotOnNUnitError();
        EventSubscriptions.TakePageSnapshotOnNUnitError();
        EventSubscriptions.AddArtifactsToNUnitTestContext();

        return this;
    }

    /// <summary>
    /// Enables all Atata features for SpecFlow+NUnit.
    /// Executes the following methods:
    /// <list type="bullet">
    /// <item><see cref="UseNUnitTestName"/></item>
    /// <item><see cref="UseNUnitTestSuiteName"/></item>
    /// <item><see cref="UseNUnitTestSuiteType"/></item>
    /// <item><see cref="UseNUnitAssertionExceptionType"/></item>
    /// <item><see cref="UseNUnitAggregateAssertionStrategy"/></item>
    /// <item><see cref="UseNUnitWarningReportStrategy"/></item>
    /// <item><see cref="UseNUnitAssertionFailureReportStrategy"/></item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.LogNUnitError(EventSubscriptionsBuilder)"/> for <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.TakeScreenshotOnNUnitError(EventSubscriptionsBuilder, string)"/> for <see cref="EventSubscriptions"/> property</item>
    /// <item><see cref="NUnitEventSubscriptionsBuilderExtensions.TakePageSnapshotOnNUnitError(EventSubscriptionsBuilder, string)"/> for <see cref="EventSubscriptions"/> property</item>
    /// </list>
    /// </summary>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder UseSpecFlowNUnitFeatures()
    {
        UseNUnitTestName();
        UseNUnitTestSuiteName();
        UseNUnitTestSuiteType();
        UseNUnitAssertionExceptionType();
        UseNUnitAggregateAssertionStrategy();
        UseNUnitWarningReportStrategy();
        UseNUnitAssertionFailureReportStrategy();
        EventSubscriptions.LogNUnitError();
        EventSubscriptions.TakeScreenshotOnNUnitError();
        EventSubscriptions.TakePageSnapshotOnNUnitError();

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
        TestInfo testInfo = new(
            TestNameFactory?.Invoke(),
            TestSuiteTypeFactory?.Invoke(),
            TestSuiteNameFactory?.Invoke(),
            TestSuiteGroupNameFactory?.Invoke());

        AtataContext parentContext = ParentContext;

        if (parentContext is null && Scope is not null && AtataContext.Global is not null)
            parentContext = AtataContextParentResolver.FindParentContext(AtataContext.Global, Scope.Value, testInfo);

        AtataContext context = new(parentContext, Scope, testInfo);
        LogManager logManager = CreateLogManager(context);

        context.Log = logManager;
        context.Attributes = Attributes.AttributesContext.Clone();
        context.BaseRetryTimeout = BaseRetryTimeout;
        context.BaseRetryInterval = BaseRetryInterval;
        context.WaitingTimeout = WaitingTimeout;
        context.WaitingRetryInterval = WaitingRetryInterval;
        context.VerificationTimeout = VerificationTimeout;
        context.VerificationRetryInterval = VerificationRetryInterval;
        context.Culture = Culture ?? CultureInfo.CurrentCulture;
        context.AssertionExceptionType = AssertionExceptionType;
        context.AggregateAssertionExceptionType = AggregateAssertionExceptionType;
        context.AggregateAssertionStrategy = AggregateAssertionStrategy ?? AtataAggregateAssertionStrategy.Instance;
        context.WarningReportStrategy = WarningReportStrategy ?? AtataWarningReportStrategy.Instance;
        context.AssertionFailureReportStrategy = AssertionFailureReportStrategy ?? AtataAssertionFailureReportStrategy.Instance;
        context.EventBus = new EventBus(context, EventSubscriptions.Items);

        context.InitMainVariables();
        context.InitCustomVariables(Variables);
        context.InitArtifactsDirectory();
        context.InitArtifactsVariable();

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
            [.. LogConsumers.Items],
            [.. SecretStringsToMaskInLog]);

        return new(configuration, new AtataContextLogEventInfoFactory(context));
    }

    private async Task InitializeContextAsync(AtataContext context, CancellationToken cancellationToken = default)
    {
        context.LogTestStart();

        try
        {
            await context.Log.ExecuteSectionAsync(
                new LogSection("Initialize AtataContext", LogLevel.Trace),
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

    private async Task DoInitializeContextAsync(AtataContext context, CancellationToken cancellationToken = default)
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
            Sessions.Providers.Select(x => (IAtataSessionProvider)x.Clone()).ToList(),
            ResolveSessionDefaultStartScopes(scope));

        copy.LogConsumers = new(
            copy,
            LogConsumers.Items.Select(x => x.Consumer is ICloneable ? x.Clone() : x).ToList());

        copy.Attributes = new AttributesBuilder(
            Attributes.AttributesContext.Clone());

        copy.EventSubscriptions = new EventSubscriptionsBuilder(
            EventSubscriptions.Items);

        copy.Variables = new Dictionary<string, object>(Variables);
        copy.SecretStringsToMaskInLog = [.. SecretStringsToMaskInLog];

        return copy;
    }

    private static AtataSessionStartScopes? ResolveSessionDefaultStartScopes(AtataContextScope? scope) =>
        scope switch
        {
            AtataContextScope.Test => AtataSessionStartScopes.Test,
            AtataContextScope.TestSuite => AtataSessionStartScopes.TestSuite,
            AtataContextScope.TestSuiteGroup => AtataSessionStartScopes.TestSuiteGroup,
            AtataContextScope.Namespace => AtataSessionStartScopes.Namespace,
            AtataContextScope.Global => AtataSessionStartScopes.Global,
            null => null,
            _ => AtataSessionStartScopes.None
        };
}
