namespace Atata;

/// <summary>
/// Represents the Atata context, the entry point for the test set-up.
/// </summary>
public sealed class AtataContext : IDisposable
{
    private static readonly object s_buildStartSyncLock = new();

    private static readonly AsyncLocal<AtataContext> s_currentAsyncLocalContext = new();

    private static AtataContextModeOfCurrent s_modeOfCurrent = AtataContextModeOfCurrent.AsyncLocal;

    [ThreadStatic]
    private static AtataContext s_currentThreadStaticContext;

    private static AtataContext s_currentStaticContext;

    private readonly AssertionVerificationStrategy _assertionVerificationStrategy;

    private readonly ExpectationVerificationStrategy _expectationVerificationStrategy;

    private IWebDriver _driver;

    private bool _disposed;

    /// <summary>
    /// Gets the base retry timeout, which is <c>5</c> seconds.
    /// </summary>
    public static readonly TimeSpan DefaultRetryTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets the default retry interval, which is <c>500</c> milliseconds.
    /// </summary>
    public static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(0.5);

    internal AtataContext()
    {
        _assertionVerificationStrategy = new AssertionVerificationStrategy(this);
        _expectationVerificationStrategy = new ExpectationVerificationStrategy(this);

        Go = new AtataNavigator(this);
        Report = new Report<AtataContext>(this, this);
    }

    /// <summary>
    /// Gets or sets the current context.
    /// </summary>
    public static AtataContext Current
    {
        get => ModeOfCurrent switch
        {
            AtataContextModeOfCurrent.AsyncLocal => s_currentAsyncLocalContext.Value,
            AtataContextModeOfCurrent.ThreadStatic => s_currentThreadStaticContext,
            _ => s_currentStaticContext
        };
        set
        {
            if (ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
                s_currentAsyncLocalContext.Value = value;
            else if (ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
                s_currentThreadStaticContext = value;
            else
                s_currentStaticContext = value;
        }
    }

    /// <summary>
    /// Gets or sets the mode of <see cref="Current"/> property.
    /// The default value is <see cref="AtataContextModeOfCurrent.AsyncLocal"/>.
    /// </summary>
    public static AtataContextModeOfCurrent ModeOfCurrent
    {
        get => s_modeOfCurrent;
        set
        {
            s_modeOfCurrent = value;

            RetrySettings.ThreadBoundary = value switch
            {
                AtataContextModeOfCurrent.AsyncLocal => RetrySettingsThreadBoundary.AsyncLocal,
                AtataContextModeOfCurrent.ThreadStatic => RetrySettingsThreadBoundary.ThreadStatic,
                _ => RetrySettingsThreadBoundary.Static
            };
        }
    }

    /// <summary>
    /// Gets the global configuration.
    /// </summary>
    public static AtataContextBuilder GlobalConfiguration { get; } = new AtataContextBuilder(new AtataBuildingContext());

    /// <summary>
    /// Gets the build start local date and time.
    /// Contains the same value for all the tests being executed within one build.
    /// </summary>
    public static DateTime? BuildStart { get; private set; }

    /// <summary>
    /// Gets the build start UTC date and time.
    /// Contains the same value for all the tests being executed within one build.
    /// </summary>
    public static DateTime? BuildStartUtc { get; private set; }

    // TODO: Review BuildStartInTimeZone property.
    internal DateTime BuildStartInTimeZone { get; private set; }

    internal IDriverFactory DriverFactory { get; set; }

    /// <summary>
    /// Gets the driver.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IWebDriver Driver
    {
        get
        {
            switch (DriverInitializationStage)
            {
                case AtataContextDriverInitializationStage.Build:
                    return _driver;
                case AtataContextDriverInitializationStage.OnDemand:
                    if (_driver is null)
                        InitDriver();
                    return _driver;
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether this instance has <see cref="Driver"/> instance.
    /// </summary>
    public bool HasDriver => _driver != null;

    /// <summary>
    /// Gets the driver alias.
    /// </summary>
    public string DriverAlias { get; internal set; }

    internal bool DisposeDriver { get; set; }

    /// <summary>
    /// Gets the driver initialization stage.
    /// </summary>
    public AtataContextDriverInitializationStage DriverInitializationStage { get; internal set; }

    /// <summary>
    /// Gets the instance of the log manager.
    /// </summary>
    public ILogManager Log { get; internal set; }

    [Obsolete("Use Test.Name instead.")] // Obsolete since v2.12.0.
    public string TestName => Test.Name;

    [Obsolete("Use Test.NameSanitized instead.")] // Obsolete since v2.12.0.
    public string TestNameSanitized => Test.NameSanitized;

    [Obsolete("Use Test.SuiteName instead.")] // Obsolete since v2.12.0.
    public string TestSuiteName => Test.SuiteName;

    [Obsolete("Use Test.SuiteNameSanitized instead.")] // Obsolete since v2.12.0.
    public string TestSuiteNameSanitized => Test.SuiteNameSanitized;

    [Obsolete("Use Test.SuiteType instead.")] // Obsolete since v2.12.0.
    public Type TestSuiteType => Test.SuiteType;

    /// <summary>
    /// Gets the test information.
    /// </summary>
    public TestInfo Test { get; } = new();

    /// <summary>
    /// Gets the local date/time of the start.
    /// </summary>
    public DateTime StartedAt { get; private set; }

    /// <summary>
    /// Gets the UTC date/time of the start.
    /// </summary>
    public DateTime StartedAtUtc { get; private set; }

    /// <summary>
    /// Gets the time zone.
    /// The default value is <see cref="TimeZoneInfo.Local"/>.
    /// </summary>
    public TimeZoneInfo TimeZone { get; internal set; }

    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Gets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan BaseRetryTimeout { get; internal set; }

    /// <summary>
    /// Gets the base retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan BaseRetryInterval { get; internal set; }

    /// <summary>
    /// Gets the element find timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan ElementFindTimeout { get; internal set; }

    /// <summary>
    /// Gets the element find retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan ElementFindRetryInterval { get; internal set; }

    /// <summary>
    /// Gets the waiting timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan WaitingTimeout { get; internal set; }

    /// <summary>
    /// Gets the waiting retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan WaitingRetryInterval { get; internal set; }

    /// <summary>
    /// Gets the verification timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan VerificationTimeout { get; internal set; }

    /// <summary>
    /// Gets the verification retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan VerificationRetryInterval { get; internal set; }

    /// <summary>
    /// Gets the default control visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    public Visibility DefaultControlVisibility { get; internal set; }

    /// <summary>
    /// Gets the culture.
    /// The default value is <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    public CultureInfo Culture { get; internal set; }

    /// <summary>
    /// Gets the type of the assertion exception.
    /// The default value is a type of <see cref="AssertionException"/>.
    /// </summary>
    public Type AssertionExceptionType { get; internal set; }

    /// <summary>
    /// Gets the type of the aggregate assertion exception.
    /// The default value is a type of <see cref="AggregateAssertionException"/>.
    /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
    /// </summary>
    public Type AggregateAssertionExceptionType { get; internal set; }

    /// <summary>
    /// Gets the aggregate assertion strategy.
    /// The default value is an instance of <see cref="AtataAggregateAssertionStrategy"/>.
    /// </summary>
    public IAggregateAssertionStrategy AggregateAssertionStrategy { get; internal set; }

    /// <summary>
    /// Gets the aggregate assertion depth level.
    /// </summary>
    public int AggregateAssertionLevel { get; internal set; }

    /// <summary>
    /// Gets the strategy for warning assertion reporting.
    /// The default value is an instance of <see cref="AtataWarningReportStrategy"/>.
    /// </summary>
    public IWarningReportStrategy WarningReportStrategy { get; internal set; }

    /// <summary>
    /// Gets the list of all assertion results.
    /// </summary>
    public List<AssertionResult> AssertionResults { get; } = new List<AssertionResult>();

    /// <summary>
    /// Gets the list of pending assertion results with <see cref="AssertionStatus.Failed"/> or <see cref="AssertionStatus.Warning"/> status.
    /// </summary>
    public List<AssertionResult> PendingFailureAssertionResults { get; } = new List<AssertionResult>();

    /// <summary>
    /// Gets the context of the attributes.
    /// </summary>
    public AtataAttributesContext Attributes { get; internal set; }

    /// <summary>
    /// Gets the <see cref="DirectorySubject"/> of Artifacts directory.
    /// Artifacts directory can contain any files produced during test execution, logs, screenshots, downloads, etc.
    /// The default Artifacts directory path is <c>"{basedir}/artifacts/{build-start:yyyyMMddTHHmmss}{test-suite-name-sanitized:/*}{test-name-sanitized:/*}"</c>.
    /// </summary>
    public DirectorySubject Artifacts { get; internal set; }

    /// <summary>
    /// Gets the <see cref="AtataNavigator"/> instance,
    /// which provides the navigation functionality between pages and windows.
    /// </summary>
    public AtataNavigator Go { get; }

    /// <summary>
    /// Gets the <see cref="Report{TOwner}"/> instance that provides a reporting functionality.
    /// </summary>
    public Report<AtataContext> Report { get; }

    /// <summary>
    /// Gets the current page object.
    /// </summary>
    public UIComponent PageObject { get; internal set; }

    internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; } = new List<UIComponent>();

    internal bool IsNavigated { get; set; }

    internal Stopwatch ExecutionStopwatch { get; } = Stopwatch.StartNew();

    internal Stopwatch BodyExecutionStopwatch { get; } = new Stopwatch();

    internal Stopwatch SetupExecutionStopwatch { get; } = new Stopwatch();

    internal ScreenshotTaker ScreenshotTaker { get; set; }

    internal PageSnapshotTaker PageSnapshotTaker { get; set; }

    public ReadOnlyCollection<UIComponent> TemporarilyPreservedPageObjects =>
        TemporarilyPreservedPageObjectList.ToReadOnly();

    /// <summary>
    /// Gets the UI component access chain scope cache.
    /// </summary>
    public UIComponentAccessChainScopeCache UIComponentAccessChainScopeCache { get; } = new UIComponentAccessChainScopeCache();

    /// <summary>
    /// Gets the object creator.
    /// </summary>
    public IObjectCreator ObjectCreator { get; internal set; }

    /// <summary>
    /// Gets the object converter.
    /// </summary>
    public IObjectConverter ObjectConverter { get; internal set; }

    /// <summary>
    /// Gets the object mapper.
    /// </summary>
    public IObjectMapper ObjectMapper { get; internal set; }

    /// <summary>
    /// Gets the event bus, which can used to subscribe to and publish events.
    /// </summary>
    public IEventBus EventBus { get; internal set; }

    /// <summary>
    /// Gets the variables dictionary.
    /// <para>
    /// The list of predefined variables:
    /// <list type="bullet">
    /// <item><c>build-start</c></item>
    /// <item><c>build-start-utc</c></item>
    /// <item><c>basedir</c></item>
    /// <item><c>artifacts</c></item>
    /// <item><c>test-name-sanitized</c></item>
    /// <item><c>test-name</c></item>
    /// <item><c>test-suite-name-sanitized</c></item>
    /// <item><c>test-suite-name</c></item>
    /// <item><c>test-start</c></item>
    /// <item><c>test-start-utc</c></item>
    /// <item><c>driver-alias</c></item>
    /// </list>
    /// </para>
    /// <para>
    /// Custom variables can be added as well.
    /// </para>
    /// </summary>
    public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets the name of the DOM test identifier attribute.
    /// The default value is <c>"data-testid"</c>.
    /// </summary>
    public string DomTestIdAttributeName { get; internal set; }

    /// <summary>
    /// Gets the default case of the DOM test identifier attribute.
    /// The default value is <see cref="TermCase.Kebab"/>.
    /// </summary>
    public TermCase DomTestIdAttributeDefaultCase { get; internal set; }

    /// <summary>
    /// Creates <see cref="AtataContextBuilder"/> instance for <see cref="AtataContext"/> configuration.
    /// Sets the value to <see cref="AtataContextBuilder.BuildingContext"/> copied from <see cref="GlobalConfiguration"/>.
    /// </summary>
    /// <returns>The created <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder Configure()
    {
        AtataBuildingContext buildingContext = GlobalConfiguration.BuildingContext.Clone();
        return new AtataContextBuilder(buildingContext);
    }

    internal void InitDateTimeProperties()
    {
        StartedAtUtc = DateTime.UtcNow;
        StartedAt = TimeZoneInfo.ConvertTimeFromUtc(StartedAtUtc, TimeZone);

        if (BuildStartUtc is null)
        {
            lock (s_buildStartSyncLock)
            {
                if (BuildStartUtc is null)
                {
                    BuildStartUtc = StartedAtUtc;
                    BuildStart = BuildStartUtc.Value.ToLocalTime();
                }
            }
        }

        BuildStartInTimeZone = TimeZoneInfo.ConvertTimeFromUtc(BuildStartUtc.Value, TimeZone);
    }

    internal void InitMainVariables()
    {
        var variables = Variables;

        variables["build-start"] = BuildStartInTimeZone;
        variables["build-start-utc"] = BuildStartUtc;

        variables["basedir"] = AppDomain.CurrentDomain.BaseDirectory;

        variables["test-name-sanitized"] = Test.NameSanitized;
        variables["test-name"] = Test.Name;
        variables["test-suite-name-sanitized"] = Test.SuiteNameSanitized;
        variables["test-suite-name"] = Test.SuiteName;
        variables["test-start"] = StartedAt;
        variables["test-start-utc"] = StartedAtUtc;

        variables["driver-alias"] = DriverAlias;
    }

    internal void InitCustomVariables(IDictionary<string, object> customVariables)
    {
        var variables = Variables;

        foreach (var variable in customVariables)
            variables[variable.Key] = variable.Value;
    }

    internal void InitArtifactsVariable() =>
        Variables["artifacts"] = Artifacts.FullName.Value;

    internal void LogTestStart()
    {
        StringBuilder logMessageBuilder = new StringBuilder(
            $"Starting {Test.GetTestUnitKindName()}");

        string testFullName = Test.FullName;

        if (testFullName is not null)
            logMessageBuilder.Append(": ").Append(testFullName);

        Log.Info(logMessageBuilder.ToString());
    }

    /// <summary>
    /// Executes aggregate assertion using <see cref="AggregateAssertionStrategy" />.
    /// </summary>
    /// <param name="action">The action to execute in scope of aggregate assertion.</param>
    /// <param name="assertionScopeName">
    /// Name of the scope being asserted (page object, control, etc.).
    /// Is used to identify the assertion section in log.
    /// Can be null.
    /// </param>
    public void AggregateAssert(Action action, string assertionScopeName = null)
    {
        action.CheckNotNull(nameof(action));

        AggregateAssertionStrategy.Assert(() =>
        {
            AggregateAssertionLevel++;

            try
            {
                Log.ExecuteSection(
                    new AggregateAssertionLogSection(assertionScopeName),
                    action);
            }
            finally
            {
                AggregateAssertionLevel--;
            }
        });
    }

    [Obsolete("Use Dispose instead.")] // Obsolete since v2.11.0.
    public void CleanUp() =>
        Dispose();

    [Obsolete("Use Dispose instead. If you don't need to quit driver, use UseDisposeDriver(false) method of AtataContextBuilder during AtataContext setup.")] // Obsolete since v2.11.0.
    public void CleanUp(bool quitDriver) =>
        DisposeTogetherWithDriver(quitDriver);

    internal void InitDriver() =>
        Log.ExecuteSection(
            new LogSection("Initialize Driver", LogLevel.Trace),
            () =>
            {
                if (DriverFactory is null)
                    throw new InvalidOperationException(
                        $"Failed to create an instance of {typeof(IWebDriver).FullName} as driver factory is not specified.");

                _driver = DriverFactory.Create()
                    ?? throw new InvalidOperationException(
                        $"Failed to create an instance of {typeof(IWebDriver).FullName} as driver factory returned null as a driver.");

                _driver.Manage().Timeouts().SetRetryTimeout(ElementFindTimeout, ElementFindRetryInterval);

                EventBus.Publish(new DriverInitEvent(_driver));
            });

    /// <summary>
    /// Restarts the driver.
    /// </summary>
    public void RestartDriver() =>
        Log.ExecuteSection(
            new LogSection("Restart driver"),
            () =>
            {
                CleanUpTemporarilyPreservedPageObjectList();

                if (PageObject != null)
                {
                    UIComponentResolver.CleanUpPageObject(PageObject);
                    PageObject = null;
                }

                EventBus.Publish(new DriverDeInitEvent(_driver));
                _driver.Dispose();

                InitDriver();
            });

    internal void CleanUpTemporarilyPreservedPageObjectList()
    {
        UIComponentResolver.CleanUpPageObjects(TemporarilyPreservedPageObjects);
        TemporarilyPreservedPageObjectList.Clear();
    }

    /// <summary>
    /// <para>
    /// Fills the template string with variables of this <see cref="AtataContext"/> instance.
    /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
    /// </para>
    /// <para>
    /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
    /// and extended formatting for strings
    /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
    /// In order to output a <c>{</c> use <c>{{</c>, and to output a <c>}</c> use <c>}}</c>.
    /// </para>
    /// <para>
    /// The list of predefined variables:
    /// <list type="bullet">
    /// <item><c>{build-start}</c></item>
    /// <item><c>{build-start-utc}</c></item>
    /// <item><c>{basedir}</c></item>
    /// <item><c>{artifacts}</c></item>
    /// <item><c>{test-name-sanitized}</c></item>
    /// <item><c>{test-name}</c></item>
    /// <item><c>{test-suite-name-sanitized}</c></item>
    /// <item><c>{test-suite-name}</c></item>
    /// <item><c>{test-start}</c></item>
    /// <item><c>{test-start-utc}</c></item>
    /// <item><c>{driver-alias}</c></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="template">The template string.</param>
    /// <returns>The filled string.</returns>
    public string FillTemplateString(string template) =>
        FillTemplateString(template, null);

    /// <inheritdoc cref="FillTemplateString(string)"/>
    /// <param name="template">The template string.</param>
    /// <param name="additionalVariables">The additional variables.</param>
    public string FillTemplateString(string template, IDictionary<string, object> additionalVariables) =>
        TransformTemplateString(template, additionalVariables, TemplateStringTransformer.Transform);

    /// <summary>
    /// <para>
    /// Fills the path template string with variables of this <see cref="AtataContext"/> instance.
    /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
    /// </para>
    /// <para>
    /// Variables are sanitized for path by replacing invalid characters with <c>'_'</c>.
    /// </para>
    /// <para>
    /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
    /// and extended formatting for strings
    /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
    /// In order to output a <c>{</c> use <c>{{</c>, and to output a <c>}</c> use <c>}}</c>.
    /// </para>
    /// <para>
    /// The list of predefined variables:
    /// <list type="bullet">
    /// <item><c>{build-start}</c></item>
    /// <item><c>{build-start-utc}</c></item>
    /// <item><c>{basedir}</c></item>
    /// <item><c>{artifacts}</c></item>
    /// <item><c>{test-name-sanitized}</c></item>
    /// <item><c>{test-name}</c></item>
    /// <item><c>{test-suite-name-sanitized}</c></item>
    /// <item><c>{test-suite-name}</c></item>
    /// <item><c>{test-start}</c></item>
    /// <item><c>{test-start-utc}</c></item>
    /// <item><c>{driver-alias}</c></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="template">The template string.</param>
    /// <returns>The filled string.</returns>
    public string FillPathTemplateString(string template) =>
        FillPathTemplateString(template, null);

    /// <inheritdoc cref="FillPathTemplateString(string)"/>
    /// <param name="template">The template string.</param>
    /// <param name="additionalVariables">The additional variables.</param>
    public string FillPathTemplateString(string template, IDictionary<string, object> additionalVariables) =>
        TransformTemplateString(template, additionalVariables, TemplateStringTransformer.TransformPath);

    /// <summary>
    /// <para>
    /// Fills the path template string with variables of this <see cref="AtataContext"/> instance.
    /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
    /// </para>
    /// <para>
    /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
    /// and extended formatting for strings
    /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
    /// In order to output a <c>{</c> use <c>{{</c>, and to output a <c>}</c> use <c>}}</c>.
    /// </para>
    /// <para>
    /// Variables are escaped by default using <see cref="Uri.EscapeDataString(string)"/> method.
    /// In order to not escape a variable, use <c>:noescape</c> modifier, for example <c>"{stringVar:noescape}"</c>.
    /// To escape a variable using <see cref="Uri.EscapeUriString(string)"/> method,
    /// preserving special URI symbols,
    /// use <c>:uriescape</c> modifier, for example <c>"{stringVar:uriescape}"</c>.
    /// Use <c>:dataescape</c> in complex scenarios (like adding optional query parameter)
    /// together with an extended formatting, for example <c>"{stringVar:dataescape:?q=*}"</c>,
    /// to escape the value and prefix it with "?q=", but nothing will be output in case
    /// <c>stringVar</c> is <see langword="null"/>.
    /// </para>
    /// <para>
    /// The list of predefined variables:
    /// <list type="bullet">
    /// <item><c>{build-start}</c></item>
    /// <item><c>{build-start-utc}</c></item>
    /// <item><c>{basedir}</c></item>
    /// <item><c>{artifacts}</c></item>
    /// <item><c>{test-name-sanitized}</c></item>
    /// <item><c>{test-name}</c></item>
    /// <item><c>{test-suite-name-sanitized}</c></item>
    /// <item><c>{test-suite-name}</c></item>
    /// <item><c>{test-start}</c></item>
    /// <item><c>{test-start-utc}</c></item>
    /// <item><c>{driver-alias}</c></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="template">The template string.</param>
    /// <returns>The filled string.</returns>
    public string FillUriTemplateString(string template) =>
        FillUriTemplateString(template, null);

    /// <inheritdoc cref="FillUriTemplateString(string)"/>
    /// <param name="template">The template string.</param>
    /// <param name="additionalVariables">The additional variables.</param>
    public string FillUriTemplateString(string template, IDictionary<string, object> additionalVariables) =>
        TransformTemplateString(template, additionalVariables, TemplateStringTransformer.TransformUri);

    private string TransformTemplateString(
        string template,
        IDictionary<string, object> additionalVariables,
        Func<string, IDictionary<string, object>, string> transformFunction)
    {
        template.CheckNotNull(nameof(template));

        if (!template.Contains('{'))
            return template;

        var variables = Variables;

        if (additionalVariables != null)
        {
            variables = new Dictionary<string, object>(variables);

            foreach (var variable in additionalVariables)
                variables[variable.Key] = variable.Value;
        }

        return transformFunction(template, variables);
    }

    /// <summary>
    /// Takes a screenshot of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a screenshot.</param>
    public void TakeScreenshot(string title = null) =>
        ScreenshotTaker?.TakeScreenshot(title);

    /// <summary>
    /// Takes a screenshot of the current page of a certain kind with an optionally specified title.
    /// </summary>
    /// <param name="kind">The kind of a screenshot.</param>
    /// <param name="title">The title of a screenshot.</param>
    public void TakeScreenshot(ScreenshotKind kind, string title = null) =>
        ScreenshotTaker?.TakeScreenshot(kind, title);

    /// <summary>
    /// Takes a snapshot (HTML or MHTML file) of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a snapshot.</param>
    public void TakePageSnapshot(string title = null) =>
        PageSnapshotTaker?.TakeSnapshot(title);

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePathWithoutExtension">The relative file path without extension.</param>
    /// <param name="fileContentWithExtension">The file content with extension.</param>
    /// <param name="artifactType">Type of the artifact. Can be a value of <see cref="ArtifactTypes" />.</param>
    /// <param name="artifactTitle">The artifact title.</param>
    public void AddArtifact(string relativeFilePathWithoutExtension, FileContentWithExtension fileContentWithExtension, string artifactType = null, string artifactTitle = null)
    {
        relativeFilePathWithoutExtension.CheckNotNullOrWhitespace(nameof(relativeFilePathWithoutExtension));
        fileContentWithExtension.CheckNotNull(nameof(fileContentWithExtension));

        string relativeFilePath = relativeFilePathWithoutExtension + fileContentWithExtension.Extension;
        string absoluteFilePath = BuildAbsoluteArtifactFilePathAndEnsureDirectoryExists(relativeFilePath);

        fileContentWithExtension.Save(absoluteFilePath);

        EventBus.Publish(new ArtifactAddedEvent(absoluteFilePath, relativeFilePath, artifactType, artifactTitle));
    }

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePath">The relative file path.</param>
    /// <param name="fileBytes">The file bytes.</param>
    /// <param name="artifactType">Type of the artifact. Can be a value of <see cref="ArtifactTypes" />.</param>
    /// <param name="artifactTitle">The artifact title.</param>
    public void AddArtifact(string relativeFilePath, byte[] fileBytes, string artifactType = null, string artifactTitle = null)
    {
        relativeFilePath.CheckNotNullOrWhitespace(nameof(relativeFilePath));
        fileBytes.CheckNotNull(nameof(fileBytes));

        string absoluteFilePath = BuildAbsoluteArtifactFilePathAndEnsureDirectoryExists(relativeFilePath);

        File.WriteAllBytes(absoluteFilePath, fileBytes);

        EventBus.Publish(new ArtifactAddedEvent(absoluteFilePath, relativeFilePath, artifactType, artifactTitle));
    }

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePath">The relative file path.</param>
    /// <param name="fileContent">Content of the file.</param>
    /// <param name="artifactType">Type of the artifact. Can be a value of <see cref="ArtifactTypes"/>.</param>
    /// <param name="artifactTitle">The artifact title.</param>
    public void AddArtifact(string relativeFilePath, string fileContent, string artifactType = null, string artifactTitle = null)
    {
        relativeFilePath.CheckNotNullOrWhitespace(nameof(relativeFilePath));
        fileContent.CheckNotNull(nameof(fileContent));

        string absoluteFilePath = BuildAbsoluteArtifactFilePathAndEnsureDirectoryExists(relativeFilePath);

        File.WriteAllText(absoluteFilePath, fileContent);

        EventBus.Publish(new ArtifactAddedEvent(absoluteFilePath, relativeFilePath, artifactType, artifactTitle));
    }

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePath">The relative file path.</param>
    /// <param name="fileContent">Content of the file.</param>
    /// <param name="encoding">The encoding. Can be <see langword="null"/>.</param>
    /// <param name="artifactType">Type of the artifact. Can be a value of <see cref="ArtifactTypes"/>.</param>
    /// <param name="artifactTitle">The artifact title.</param>
    public void AddArtifact(string relativeFilePath, string fileContent, Encoding encoding, string artifactType = null, string artifactTitle = null)
    {
        relativeFilePath.CheckNotNullOrWhitespace(nameof(relativeFilePath));
        fileContent.CheckNotNull(nameof(fileContent));

        string absoluteFilePath = BuildAbsoluteArtifactFilePathAndEnsureDirectoryExists(relativeFilePath);

        if (encoding is null)
            File.WriteAllText(absoluteFilePath, fileContent);
        else
            File.WriteAllText(absoluteFilePath, fileContent, encoding);

        EventBus.Publish(new ArtifactAddedEvent(absoluteFilePath, relativeFilePath, artifactType, artifactTitle));
    }

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePath">The relative file path.</param>
    /// <param name="stream">The stream to write to the file.</param>
    /// <param name="artifactType">Type of the artifact. Can be a value of <see cref="ArtifactTypes" />.</param>
    /// <param name="artifactTitle">The artifact title.</param>
    public void AddArtifact(string relativeFilePath, Stream stream, string artifactType = null, string artifactTitle = null)
    {
        relativeFilePath.CheckNotNullOrWhitespace(nameof(relativeFilePath));
        stream.CheckNotNull(nameof(stream));

        string absoluteFilePath = BuildAbsoluteArtifactFilePathAndEnsureDirectoryExists(relativeFilePath);

        using (FileStream source = File.Create(absoluteFilePath))
            stream.CopyTo(source);

        EventBus.Publish(new ArtifactAddedEvent(absoluteFilePath, relativeFilePath, artifactType, artifactTitle));
    }

    private string BuildAbsoluteArtifactFilePathAndEnsureDirectoryExists(string relativeFilePath)
    {
        string absoluteFilePath = Path.Combine(Artifacts.FullName, relativeFilePath);
        string directoryPath = Path.GetDirectoryName(absoluteFilePath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        return absoluteFilePath;
    }

    /// <summary>
    /// Raises the error by throwing an assertion exception.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The optional exception.</param>
    public void RaiseError(string message, Exception exception = null) =>
        _assertionVerificationStrategy.ReportFailure(message, exception);

    /// <summary>
    /// Raises the warning by recording an assertion warning.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The optional exception.</param>
    public void RaiseWarning(string message, Exception exception = null) =>
        _expectationVerificationStrategy.ReportFailure(message, exception);

    /// <summary>
    /// Sets this context as current, by setting it to <see cref="Current"/> property.
    /// </summary>
    public void SetAsCurrent()
    {
        if (s_modeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
        {
            if (s_currentAsyncLocalContext.Value != this)
                s_currentAsyncLocalContext.Value = this;
        }
        else if (s_modeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
        {
            if (s_currentThreadStaticContext != this)
                s_currentThreadStaticContext = this;
        }
        else
        {
            if (s_currentStaticContext != this)
                s_currentStaticContext = this;
        }
    }

    /// <summary>
    /// Deinitializes and disposes the current instance and related objects.
    /// Also writes the execution time to log;
    /// throws <see cref="AggregateAssertionException"/> if
    /// <see cref="PendingFailureAssertionResults"/> is not empty (contains warnings).
    /// If <see cref="AtataBuildingContext.DisposeDriver"/> property is set to <see langword="true"/> (by default),
    /// then the <see cref="Driver"/> will also be disposed.
    /// Publishes events: <see cref="AtataContextCleanUpEvent"/>, <see cref="DriverDeInitEvent"/>.
    /// </summary>
    public void Dispose() =>
        DisposeTogetherWithDriver(DisposeDriver);

    private void DisposeTogetherWithDriver(bool disposeDriver)
    {
        if (_disposed)
            return;

        BodyExecutionStopwatch.Stop();
        Stopwatch deinitializationStopwatch = Stopwatch.StartNew();

        Log.ExecuteSection(
            new LogSection("Deinitialize AtataContext", LogLevel.Trace),
            () =>
            {
#pragma warning disable CS0618 // Type or member is obsolete
                EventBus.Publish(new AtataContextCleanUpEvent(this));
#pragma warning restore CS0618 // Type or member is obsolete

                EventBus.Publish(new AtataContextDeInitEvent(this));

                CleanUpTemporarilyPreservedPageObjectList();

                if (PageObject != null)
                    UIComponentResolver.CleanUpPageObject(PageObject);

                UIComponentAccessChainScopeCache.Release();

                EventBus.Publish(new DriverDeInitEvent(_driver));

                if (disposeDriver)
                    _driver?.Dispose();

                EventBus.Publish(new AtataContextDeInitCompletedEvent(this));
            });

        deinitializationStopwatch.Stop();
        ExecutionStopwatch.Stop();

        LogTestFinish(deinitializationStopwatch.Elapsed);

        Log = null;

        if (Current == this)
            Current = null;

        _disposed = true;

        AssertionResults.Clear();

        if (PendingFailureAssertionResults.Any())
        {
            var copyOfPendingFailureAssertionResults = PendingFailureAssertionResults.ToArray();
            PendingFailureAssertionResults.Clear();

            throw VerificationUtils.CreateAggregateAssertionException(copyOfPendingFailureAssertionResults);
        }
    }

    private void LogTestFinish(TimeSpan deinitializationTime)
    {
        string testUnitKindName = Test.GetTestUnitKindName();

        TimeSpan overallTime = ExecutionStopwatch.Elapsed;
        TimeSpan setupTime = SetupExecutionStopwatch.Elapsed;
        TimeSpan testBodyTime = BodyExecutionStopwatch.Elapsed;
        TimeSpan initializationTime = overallTime - setupTime - testBodyTime - deinitializationTime;

        string totalTimeString = overallTime.ToLongIntervalString();
        string initializationTimeString = initializationTime.ToLongIntervalString();
        string setupTimeString = setupTime.ToLongIntervalString();
        string testBodyTimeString = testBodyTime.ToLongIntervalString();
        string deinitializationTimeString = deinitializationTime.ToLongIntervalString();

        int maxTimeStringLength = new[]
        {
            totalTimeString.Length,
            initializationTimeString.Length,
            setupTimeString.Length,
            testBodyTimeString.Length,
            deinitializationTimeString.Length
        }.Max();

        double initializationTimePercent = initializationTime.TotalMilliseconds / overallTime.TotalMilliseconds;
        double setupTimePercent = setupTime.TotalMilliseconds / overallTime.TotalMilliseconds;
        double deinitializationTimePercent = deinitializationTime.TotalMilliseconds / overallTime.TotalMilliseconds;
        double testBodyPercent = 1 - initializationTimePercent - setupTimePercent - deinitializationTimePercent;

        const string percentFormat = "P1";
        CultureInfo percentCulture = CultureInfo.InvariantCulture;
        string initializationTimePercentString = initializationTimePercent.ToString(percentFormat, percentCulture);
        string setupTimePercentString = setupTimePercent.ToString(percentFormat, percentCulture);
        string deinitializationTimePercentString = deinitializationTimePercent.ToString(percentFormat, percentCulture);
        string testBodyPercentString = testBodyPercent.ToString(percentFormat, percentCulture);

        int maxPercentStringLength = new[]
        {
            initializationTimePercentString.Length,
            setupTimePercentString.Length,
            deinitializationTimePercentString.Length,
            testBodyPercentString.Length
        }.Max();

        var messageBuilder = new StringBuilder(
            $"""
            Finished {testUnitKindName}
                  Total time: {totalTimeString.PadLeft(maxTimeStringLength)}
              Initialization: {initializationTimeString.PadLeft(maxTimeStringLength)} | {initializationTimePercentString.PadLeft(maxPercentStringLength)}
            """);

        if (setupTime > TimeSpan.Zero)
            messageBuilder.AppendLine().Append(
                $"           Setup: {setupTimeString.PadLeft(maxTimeStringLength)} | {setupTimePercentString.PadLeft(maxPercentStringLength)}");

        messageBuilder.AppendLine().Append(
            $"""
            {$"{testUnitKindName.ToUpperFirstLetter()} body:",17} {testBodyTimeString.PadLeft(maxTimeStringLength)} | {testBodyPercentString.PadLeft(maxPercentStringLength)}
            Deinitialization: {deinitializationTimeString.PadLeft(maxTimeStringLength)} | {deinitializationTimePercentString.PadLeft(maxPercentStringLength)}
            """);

        Log.Info(messageBuilder.ToString());
    }
}
