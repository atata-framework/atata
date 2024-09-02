namespace Atata;

/// <summary>
/// Represents the building context for <see cref="AtataContext"/> creation.
/// It is used by <see cref="AtataContextBuilder"/>.
/// </summary>
public class AtataBuildingContext : ICloneable
{
    public const string DefaultArtifactsPathTemplate = "{test-suite-name-sanitized:/*}{test-name-sanitized:/*}";

    private TimeSpan? _waitingTimeout;

    private TimeSpan? _waitingRetryInterval;

    private TimeSpan? _verificationTimeout;

    private TimeSpan? _verificationRetryInterval;

    internal AtataBuildingContext()
    {
    }

    /// <summary>
    /// Gets the log consumer configurations.
    /// </summary>
    public List<LogConsumerConfiguration> LogConsumerConfigurations { get; private set; } = [];

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
    /// Gets the context of the attributes.
    /// </summary>
    public AtataAttributesContext Attributes { get; private set; } = new AtataAttributesContext();

    /// <summary>
    /// Gets the list of event subscriptions.
    /// </summary>
    public List<EventSubscriptionItem> EventSubscriptions { get; private set; } = [];

    /// <summary>
    /// Gets or sets the default assembly name pattern that is used to filter assemblies to find types in them.
    /// The default value is <c>@"^(?!System($|\..+$)|mscorlib$|netstandard$|Microsoft\..+)"</c>, which filters non-system assemblies.
    /// </summary>
    public string DefaultAssemblyNamePatternToFindTypes { get; set; } = @"^(?!System($|\..+)|mscorlib$|netstandard$|Microsoft\..+)";

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find component types in them.
    /// The default value is <see langword="null"/>, which means to use <see cref="DefaultAssemblyNamePatternToFindTypes"/>.
    /// </summary>
    public string AssemblyNamePatternToFindComponentTypes { get; set; }

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find attribute types in them.
    /// The default value is <see langword="null"/>, which means to use <see cref="DefaultAssemblyNamePatternToFindTypes"/>.
    /// </summary>
    public string AssemblyNamePatternToFindAttributeTypes { get; set; }

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find event types in them.
    /// The default value is <see langword="null"/>, which means to use <see cref="DefaultAssemblyNamePatternToFindTypes"/>.
    /// </summary>
    public string AssemblyNamePatternToFindEventTypes { get; set; }

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find event handler types in them.
    /// The default value is <see langword="null"/>, which means to use <see cref="DefaultAssemblyNamePatternToFindTypes"/>.
    /// </summary>
    public string AssemblyNamePatternToFindEventHandlerTypes { get; set; }

    /// <summary>
    /// Gets or sets the Artifacts directory path template.
    /// The default value is <c>"{test-suite-name-sanitized:/*}{test-name-sanitized:/*}"</c>.
    /// </summary>
    public string ArtifactsPathTemplate { get; set; } = DefaultArtifactsPathTemplate;

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
    public IAggregateAssertionStrategy AggregateAssertionStrategy { get; set; } = new AtataAggregateAssertionStrategy();

    /// <summary>
    /// Gets or sets the strategy for warning assertion reporting.
    /// The default value is an instance of <see cref="AtataWarningReportStrategy"/>.
    /// </summary>
    public IWarningReportStrategy WarningReportStrategy { get; set; } = new AtataWarningReportStrategy();

    /// <summary>
    /// Gets or sets the strategy for assertion failure reporting.
    /// The default value is an instance of <see cref="AtataAssertionFailureReportStrategy"/>.
    /// </summary>
    public IAssertionFailureReportStrategy AssertionFailureReportStrategy { get; set; } = AtataAssertionFailureReportStrategy.Instance;

    /// <summary>
    /// Gets the configuration of browser logs monitoring and handling.
    /// </summary>
    public BrowserLogsConfiguration BrowserLogs { get; private set; } = new BrowserLogsConfiguration();

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() =>
        Clone();

    /// <summary>
    /// Creates a copy of the current instance.
    /// </summary>
    /// <returns>The copied <see cref="AtataBuildingContext"/> instance.</returns>
    public AtataBuildingContext Clone()
    {
        AtataBuildingContext copy = (AtataBuildingContext)MemberwiseClone();

        copy.LogConsumerConfigurations = LogConsumerConfigurations
            .Select(x => x.Consumer is ICloneable ? x.Clone() : x)
            .ToList();

        copy.Attributes = Attributes.Clone();
        copy.EventSubscriptions = [.. EventSubscriptions];
        copy.Variables = new Dictionary<string, object>(Variables);
        copy.SecretStringsToMaskInLog = [.. SecretStringsToMaskInLog];
        copy.BrowserLogs = BrowserLogs.Clone();

        return copy;
    }
}
