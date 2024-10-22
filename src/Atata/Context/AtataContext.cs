namespace Atata;

/// <summary>
/// Represents the context of a test scope (test, test suite, global test context).
/// </summary>
public sealed class AtataContext : IDisposable
{
    private static readonly AsyncLocal<AtataContext> s_currentAsyncLocalContext = new();

    [ThreadStatic]
    private static AtataContext s_currentThreadStaticContext;

    private static AtataContext s_currentStaticContext;

    private bool _disposed;

    /// <summary>
    /// Gets the base retry timeout, which is <c>5</c> seconds.
    /// </summary>
    public static readonly TimeSpan DefaultRetryTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets the default retry interval, which is <c>500</c> milliseconds.
    /// </summary>
    public static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(0.5);

    private readonly List<AtataContext> _childContexts = [];

    internal AtataContext(AtataContext parentContext, AtataContextScope? scope, TestInfo testInfo)
    {
        ParentContext = parentContext;
        Scope = scope;
        Test = testInfo;

        Id = GlobalProperties.IdGenerator.GenerateId();
        ExecutionUnit = new AtataContextExecutionUnit(this);

        Report = new Report<AtataContext>(this, ExecutionUnit);

        Variables = new(parentContext?.Variables);
        State = new(parentContext?.State);
    }

    /// <summary>
    /// Gets or sets the current context.
    /// </summary>
    public static AtataContext Current
    {
        get => GlobalProperties.ModeOfCurrent switch
        {
            AtataContextModeOfCurrent.AsyncLocal => s_currentAsyncLocalContext.Value,
            AtataContextModeOfCurrent.ThreadStatic => s_currentThreadStaticContext,
            _ => s_currentStaticContext
        };
        set
        {
            if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
                s_currentAsyncLocalContext.Value = value;
            else if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
                s_currentThreadStaticContext = value;
            else
                s_currentStaticContext = value;
        }
    }

    /// <summary>
    /// Gets the global properties that should be configured as early as possible,
    /// typically in global setup method
    /// before any creation of <see cref="AtataContext"/>, and not changed later,
    /// because these properties should have the same values for all the contexts within an execution.
    /// </summary>
    public static AtataContextGlobalProperties GlobalProperties { get; } = new();

    /// <summary>
    /// Gets the global <see cref="AtataContext"/>.
    /// Use <see cref="CreateBuilder(AtataContextScope)"/> method with <see cref="AtataContextScope.Global"/> value
    /// to register one <see cref="AtataContext"/> as a global one.
    /// </summary>
    public static AtataContext Global { get; private set; }

    [Obsolete("Use BaseConfiguration instead.")] // Obsolete since v4.0.0.
    public static AtataContextBuilder GlobalConfiguration => BaseConfiguration;

    /// <summary>
    /// Gets the base configuration builder.
    /// </summary>
    public static AtataContextBuilder BaseConfiguration { get; } =
        new(contextScope: null, sessionStartScopes: AtataSessionStartScopes.None);

    [Obsolete("Use AtataContext.GlobalProperties.ObjectConverter instead.")] // Obsolete since v4.0.0.
    public IObjectConverter ObjectConverter =>
        GlobalProperties.ObjectConverter;

    [Obsolete("Use AtataContext.GlobalProperties.ObjectMapper instead.")] // Obsolete since v4.0.0.
    public IObjectMapper ObjectMapper =>
        GlobalProperties.ObjectMapper;

    [Obsolete("Use AtataContext.GlobalProperties.ObjectCreator instead.")] // Obsolete since v4.0.0.
    public IObjectCreator ObjectCreator =>
        GlobalProperties.ObjectCreator;

    /// <summary>
    /// Gets the parent <see cref="AtataContext"/> instance or <see langword="null"/>.
    /// </summary>
    public AtataContext ParentContext { get; }

    /// <summary>
    /// Gets the child <see cref="AtataContext"/> instances of this context.
    /// </summary>
    public IReadOnlyList<AtataContext> ChildContexts => _childContexts;

    /// <summary>
    /// Gets the scope of context.
    /// </summary>
    public AtataContextScope? Scope { get; }

    /// <summary>
    /// Gets the test information.
    /// </summary>
    public TestInfo Test { get; }

    /// <summary>
    /// Gets the execution unit.
    /// </summary>
    public IAtataExecutionUnit ExecutionUnit { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is active (not disposed).
    /// </summary>
    public bool IsActive => !_disposed;

    /// <summary>
    /// Gets the unique context identifier.
    /// </summary>
    public string Id { get; }

    public AtataSessionCollection Sessions { get; } = [];

    [Obsolete("Use GetWebDriverSession().DriverFactory instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal IWebDriverFactory DriverFactory { get; set; }

    [Obsolete("Use GetWebDriver() or GetWebDriverSession().Driver instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IWebDriver Driver =>
        this.GetWebDriverSession().Driver;

    // TODO: Change HasDriver obsolete message.
    [Obsolete("Use GetWebDriverSession().HasDriver instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasDriver =>
        this.GetWebDriverSession().HasDriver;

    [Obsolete("Use GetWebDriverSession().DriverAlias instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string DriverAlias =>
        this.GetWebDriverSession().DriverAlias;

    /// <summary>
    /// Gets the instance of the log manager.
    /// </summary>
    public ILogManager Log { get; internal set; }

    /// <summary>
    /// Gets the local date/time of the start.
    /// </summary>
    public DateTime StartedAt { get; private set; }

    /// <summary>
    /// Gets the UTC date/time of the start.
    /// </summary>
    public DateTime StartedAtUtc { get; private set; }

    [Obsolete("Use GetWebSession().BaseUrl instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string BaseUrl =>
        this.GetWebSession().BaseUrl;

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

    [Obsolete("Use GetWebSession().ElementFindTimeout instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TimeSpan ElementFindTimeout =>
        this.GetWebSession().ElementFindTimeout;

    [Obsolete("Use GetWebSession().ElementFindRetryInterval instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TimeSpan ElementFindRetryInterval =>
        this.GetWebSession().ElementFindRetryInterval;

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

    [Obsolete("Use GetWebDriverSession().DefaultControlVisibility instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Visibility DefaultControlVisibility =>
        this.GetWebDriverSession().DefaultControlVisibility;

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
    /// Gets the strategy for assertion failure reporting.
    /// The default value is an instance of <see cref="AtataAssertionFailureReportStrategy"/>.
    /// </summary>
    public IAssertionFailureReportStrategy AssertionFailureReportStrategy { get; internal set; }

    /// <summary>
    /// Gets the list of all assertion results.
    /// </summary>
    public List<AssertionResult> AssertionResults { get; } = [];

    /// <summary>
    /// Gets the list of pending assertion results with <see cref="AssertionStatus.Failed"/> or <see cref="AssertionStatus.Warning"/> status.
    /// </summary>
    public List<AssertionResult> PendingFailureAssertionResults { get; } = [];

    internal Exception LastLoggedException { get; set; }

    /// <summary>
    /// Gets the context of the attributes.
    /// </summary>
    public AtataAttributesContext Attributes { get; internal set; }

    /// <summary>
    /// Gets the <see cref="DirectorySubject"/> of Artifacts directory.
    /// Artifacts directory can contain any files produced during test execution, logs, screenshots, downloads, etc.
    /// The default Artifacts directory path is <c>"{test-suite-name-sanitized:/*}{test-name-sanitized:/*}"</c>
    /// relative to <see cref="AtataContextGlobalProperties.ArtifactsRootPath"/> value
    /// of <see cref="GlobalProperties"/>.
    /// </summary>
    public DirectorySubject Artifacts { get; internal set; }

    /// <summary>
    /// Gets the path of Artifacts directory.
    /// Artifacts directory can contain any files produced during test execution, logs, screenshots, downloads, etc.
    /// The default Artifacts directory path is <c>"{test-suite-name-sanitized:/*}{test-name-sanitized:/*}"</c>
    /// relative to <see cref="AtataContextGlobalProperties.ArtifactsRootPath"/> value
    /// of <see cref="GlobalProperties"/>.
    /// </summary>
    public string ArtifactsPath => Artifacts?.FullName.Value;

    [Obsolete("Use GetWebSession().Go instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AtataNavigator Go =>
        this.GetWebSession().Go;

    /// <summary>
    /// Gets the <see cref="IReport{TOwner}"/> instance that provides a reporting functionality.
    /// </summary>
    public IReport<AtataContext> Report { get; }

    [Obsolete("Use GetWebSession().PageObject instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public UIComponent PageObject =>
        this.GetWebSession().PageObject;

    internal Stopwatch ExecutionStopwatch { get; } = Stopwatch.StartNew();

    internal Stopwatch BodyExecutionStopwatch { get; } = new Stopwatch();

    internal Stopwatch SetupExecutionStopwatch { get; } = new Stopwatch();

    [Obsolete("Use GetWebDriverSession().TemporarilyPreservedPageObjects instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IReadOnlyList<UIComponent> TemporarilyPreservedPageObjects =>
        this.GetWebDriverSession().TemporarilyPreservedPageObjects;

    [Obsolete("Use GetWebDriverSession().UIComponentAccessChainScopeCache instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public UIComponentAccessChainScopeCache UIComponentAccessChainScopeCache =>
        this.GetWebDriverSession().UIComponentAccessChainScopeCache;

    /// <summary>
    /// Gets the event bus of <see cref="AtataContext"/>,
    /// which can used to subscribe to and publish events.
    /// </summary>
    public IEventBus EventBus { get; internal set; }

    /// <summary>
    /// <para>
    /// Gets the variables hierarchical dictionary of this context.
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
    /// </list>
    /// <para>
    /// Custom variables can be added as well.
    /// </para>
    /// </summary>
    public VariableHierarchicalDictionary Variables { get; }

    /// <summary>
    /// Gets the state hierarchical dictionary of this context.
    /// By default the dictionary is empty.
    /// </summary>
    public StateHierarchicalDictionary State { get; }

    [Obsolete("Use GetWebSession().DomTestIdAttributeName instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string DomTestIdAttributeName =>
        this.GetWebSession().DomTestIdAttributeName;

    [Obsolete("Use GetWebSession().DomTestIdAttributeDefaultCase instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TermCase DomTestIdAttributeDefaultCase =>
        this.GetWebSession().DomTestIdAttributeDefaultCase;

    /// <summary>
    /// Gets the current <see cref="AtataContext"/> instance.
    /// If it's missing (<see cref="Current"/> is <see langword="null"/>), throws <see cref="AtataContextNotFoundException"/>.
    /// </summary>
    /// <returns>An <see cref="AtataContext"/> instance.</returns>
    public static AtataContext ResolveCurrent() =>
        Current ?? throw AtataContextNotFoundException.Create();

    [Obsolete("Use CreateBuilder(...) instead.")] // Obsolete since v4.0.0.
    public static AtataContextBuilder Configure() =>
        CreateBuilder(AtataContextScope.Test);

    /// <summary>
    /// Creates <see cref="AtataContextBuilder"/> instance for <see cref="AtataContext"/> configuration.
    /// The builder is a copy of <see cref="BaseConfiguration"/>, with the specified
    /// <paramref name="scope"/> as a <see cref="AtataContextBuilder.Scope"/> of the new builder.
    /// </summary>
    /// <param name="scope">The scope of context.</param>
    /// <returns>The created <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder CreateBuilder(AtataContextScope scope)
    {
        ValidateNewBuilderScope(scope);
        return BaseConfiguration.CloneFor(scope);
    }

    /// <summary>
    /// Creates default <see cref="AtataContextBuilder"/> instance for <see cref="AtataContext"/> configuration.
    /// The builder is a new instance of <see cref="AtataContextBuilder"/> class, with the specified
    /// <paramref name="scope"/> as a <see cref="AtataContextBuilder.Scope"/> of the new builder.
    /// </summary>
    /// <param name="scope">The scope of context.</param>
    /// <returns>The created <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder CreateDefaultBuilder(AtataContextScope scope)
    {
        ValidateNewBuilderScope(scope);
        return new(scope);
    }

    /// <summary>
    /// Creates <see cref="AtataContextBuilder"/> instance for <see cref="AtataContext"/> configuration.
    /// The builder is a copy of <see cref="BaseConfiguration"/>, with
    /// <see langword="null"/> as a <see cref="AtataContextBuilder.Scope"/> of the new builder.
    /// </summary>
    /// <returns>The created <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder CreateNonScopedBuilder() =>
        BaseConfiguration.CopyFor(scope: null);

    /// <summary>
    /// Creates default <see cref="AtataContextBuilder"/> instance for <see cref="AtataContext"/> configuration.
    /// The builder is a new instance of <see cref="AtataContextBuilder"/> class, with
    /// <see langword="null"/> as a <see cref="AtataContextBuilder.Scope"/> of the new builder.
    /// </summary>
    /// <returns>The created <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder CreateDefaultNonScopedBuilder() =>
        new(contextScope: null, sessionStartScopes: null);

    private static void ValidateNewBuilderScope(AtataContextScope scope)
    {
        if (scope == AtataContextScope.Global && Global is not null)
            throw new InvalidOperationException(
                $"{nameof(AtataContext)}.{nameof(Global)} is already set. There can be only one global context configured.");
    }

    internal void AddChildContext(AtataContext context)
    {
        lock (_childContexts)
        {
            _childContexts.Add(context);
        }
    }

    internal void InitDateTimeProperties()
    {
        StartedAtUtc = DateTime.UtcNow;
        StartedAt = TimeZoneInfo.ConvertTimeFromUtc(StartedAtUtc, GlobalProperties.TimeZone);
    }

    internal void InitMainVariables()
    {
        var variables = Variables;

        variables.SetInitialValue("execution-unit-id", Id);
        variables.SetInitialValue("context-id", Id);
        variables.SetInitialValue("test-name-sanitized", Test.NameSanitized);
        variables.SetInitialValue("test-name", Test.Name);
        variables.SetInitialValue("test-suite-name-sanitized", Test.SuiteNameSanitized);
        variables.SetInitialValue("test-suite-name", Test.SuiteName);
        variables.SetInitialValue("test-start", StartedAt);
        variables.SetInitialValue("test-start-utc", StartedAtUtc);
    }

    internal void InitCustomVariables(IEnumerable<KeyValuePair<string, object>> customVariables)
    {
        var variables = Variables;

        foreach (var variable in customVariables)
            variables.SetInitialValue(variable.Key, variable.Value);
    }

    internal void InitArtifactsVariable() =>
        Variables.SetInitialValue("artifacts", ArtifactsPath);

    internal void LogTestStart()
    {
        StringBuilder logMessageBuilder = new(
            $"Starting {Test.GetTestUnitKindName()}");

        string testFullName = Test.FullName;

        if (testFullName is not null)
            logMessageBuilder.Append(": ").Append(testFullName);

        Log.Debug(logMessageBuilder.ToString());
    }

    /// <summary>
    /// Executes an aggregate assertion using <see cref="AggregateAssertionStrategy" />.
    /// </summary>
    /// <param name="action">The action to execute in scope of aggregate assertion.</param>
    /// <param name="assertionScopeName">
    /// Name of the scope being asserted.
    /// Is used to identify the assertion section in log.
    /// Can be <see langword="null"/>.
    /// </param>
    public void AggregateAssert(Action action, string assertionScopeName = null) =>
        AggregateAssert(action, Log, assertionScopeName);

    internal void AggregateAssert(Action action, ILogManager log, string assertionScopeName = null)
    {
        action.CheckNotNull(nameof(action));

        try
        {
            AggregateAssertionStrategy.Assert(
                ExecutionUnit,
                () =>
                {
                    AggregateAssertionLevel++;

                    try
                    {
                        log.ExecuteSection(
                            new AggregateAssertionLogSection(assertionScopeName),
                            () =>
                            {
                                try
                                {
                                    action.Invoke();
                                }
                                catch (Exception exception)
                                {
                                    EnsureExceptionIsLogged(exception, log);
                                    throw;
                                }
                            });
                    }
                    finally
                    {
                        AggregateAssertionLevel--;
                    }
                });
        }
        catch (Exception exception)
        {
            LastLoggedException = exception;
            throw;
        }
    }

    internal void EnsureExceptionIsLogged(Exception exception, ILogManager log = null)
    {
        if (exception != LastLoggedException)
        {
            (log ?? Log).Error(exception.ToString());
            LastLoggedException = exception;
        }
    }

    [Obsolete("Use GetWebDriverSession().RestartDriver() instead.")] // Obsolete since v4.0.0.
    public void RestartDriver() =>
        this.GetWebDriverSession().RestartDriver();

    [Obsolete("Use GetWebSession().TakeScreenshot(string) instead.")] // Obsolete since v4.0.0.
    public void TakeScreenshot(string title = null) =>
        this.GetWebSession().TakeScreenshot(title);

    [Obsolete("Use GetWebSession().TakeScreenshot(ScreenshotKind, string) instead.")] // Obsolete since v4.0.0.
    public void TakeScreenshot(ScreenshotKind kind, string title = null) =>
        this.GetWebSession().TakeScreenshot(kind, title);

    [Obsolete("Use GetWebSession().TakePageSnapshot(string) instead.")] // Obsolete since v4.0.0.
    public void TakePageSnapshot(string title = null) =>
        this.GetWebSession().TakePageSnapshot(title);

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
        string absoluteFilePath = Path.Combine(ArtifactsPath, relativeFilePath);
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
        AssertionVerificationStrategy.Instance.ReportFailure(ExecutionUnit, message, exception);

    /// <summary>
    /// Raises the warning by recording an assertion warning.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The optional exception.</param>
    public void RaiseWarning(string message, Exception exception = null) =>
        ExpectationVerificationStrategy.Instance.ReportFailure(ExecutionUnit, message, exception);

    /// <summary>
    /// Sets this context as current, by setting it to <see cref="Current"/> property.
    /// </summary>
    public void SetAsCurrent()
    {
        if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
        {
            if (s_currentAsyncLocalContext.Value != this)
                s_currentAsyncLocalContext.Value = this;
        }
        else if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
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
    /// Publishes events: <see cref="AtataContextDeInitEvent"/>, <see cref="AtataSessionDeInitEvent"/> (for sessions), <see cref="AtataContextDeInitCompletedEvent"/>.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        BodyExecutionStopwatch.Stop();
        Stopwatch deinitializationStopwatch = Stopwatch.StartNew();

        Log.ExecuteSection(
            new LogSection("Deinitialize AtataContext", LogLevel.Trace),
            () =>
            {
                EventBus.Publish(new AtataContextDeInitEvent(this));

                foreach (var session in Sessions)
                    session.Deactivate();

#warning Review sessions disposing.
                foreach (var session in Sessions)
                    session.Dispose();

                Sessions.Dispose();

                EventBus.Publish(new AtataContextDeInitCompletedEvent(this));

                EventBus.UnsubscribeAll();
                State.Clear();
            });

        deinitializationStopwatch.Stop();
        ExecutionStopwatch.Stop();

        LogTestFinish(deinitializationStopwatch.Elapsed);

        Variables.Clear();
        Log = null;

        if (Current == this)
            Current = null;

        _disposed = true;

        AssertionResults.Clear();

        if (PendingFailureAssertionResults.Any())
        {
            var pendingFailureAssertionResults = GetAndClearPendingFailureAssertionResults();
            throw VerificationUtils.CreateAggregateAssertionException(this, pendingFailureAssertionResults);
        }
    }

    internal IReadOnlyList<AssertionResult> GetAndClearPendingFailureAssertionResults()
    {
        var copyOfPendingFailureAssertionResults = PendingFailureAssertionResults.ToArray();
        PendingFailureAssertionResults.Clear();
        return copyOfPendingFailureAssertionResults;
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
        double testBodyPercent = Math.Max(1d - initializationTimePercent - setupTimePercent - deinitializationTimePercent, 0);

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

        Log.Debug(messageBuilder.ToString());
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var builder = new StringBuilder(GetType().Name)
            .Append(" { Id=")
            .Append(Id);

        if (Test.FullName is not null)
            builder.Append(", Test=")
                .Append(Test.FullName);

        builder.Append(" }");

        return builder.ToString();
    }
}
