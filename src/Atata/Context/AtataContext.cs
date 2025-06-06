﻿namespace Atata;

/// <summary>
/// Represents the context of a test scope (test, test suite, global test context).
/// </summary>
public sealed class AtataContext : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the base retry timeout, which is <c>5</c> seconds.
    /// </summary>
    public static readonly TimeSpan DefaultRetryTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets the default retry interval, which is <c>200</c> milliseconds.
    /// </summary>
    public static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(0.2);

    private static readonly AsyncLocal<AtataContext?> s_currentAsyncLocalContext = new();

    private static readonly AsyncLocal<StrongBox<AtataContext?>> s_currentAsyncLocalBoxedContext = new();

    [ThreadStatic]
    private static AtataContext? s_currentThreadStaticContext;

    private static AtataContext? s_currentStaticContext;

    private readonly Lazy<string> _lazyStringRepresentation;

    private readonly Lazy<string> _lazyShortStringRepresentation;

    private readonly AddOnlyList<AtataContext> _childContexts = [];

    private Status _status;

    private int _artifactNumber;

    internal AtataContext(AtataContext? parentContext, AtataContextScope? scope, TestInfo testInfo)
    {
        _lazyStringRepresentation = new(ToStringCore, LazyThreadSafetyMode.None);
        _lazyShortStringRepresentation = new(ToShortStringCore, LazyThreadSafetyMode.None);

        ParentContext = parentContext;
        Scope = scope;
        Test = testInfo;

        Id = GlobalProperties.IdGenerator.GenerateId();
        ExecutionUnit = new AtataContextExecutionUnit(this);

        Sessions = new(this);

        Report = new Report<AtataContext>(this, ExecutionUnit);

        Variables = new(parentContext?.Variables);
        State = new(parentContext?.State);

        StartedAtUtc = DateTime.UtcNow;
        StartedAt = GlobalProperties.ConvertToTimeZone(StartedAtUtc);
    }

    private enum Status
    {
        Created,
        Active,
        Disposed
    }

    /// <summary>
    /// Gets or sets the current context.
    /// </summary>
    public static AtataContext? Current
    {
        get => GlobalProperties.ModeOfCurrent switch
        {
            AtataContextModeOfCurrent.AsyncLocal => s_currentAsyncLocalContext.Value,
            AtataContextModeOfCurrent.AsyncLocalBoxed => s_currentAsyncLocalBoxedContext.Value?.Value,
            AtataContextModeOfCurrent.ThreadStatic => s_currentThreadStaticContext,
            _ => s_currentStaticContext
        };
        set
        {
            if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
            {
                s_currentAsyncLocalContext.Value = value;
            }
            else if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocalBoxed)
            {
                var currentAsyncLocalBoxedContextValue = s_currentAsyncLocalBoxedContext.Value;

                if (value is null)
                {
                    if (currentAsyncLocalBoxedContextValue is not null)
                    {
                        currentAsyncLocalBoxedContextValue.Value = null;
                    }
                }
                else
                {
                    if (currentAsyncLocalBoxedContextValue is null)
                    {
                        s_currentAsyncLocalBoxedContext.Value = new(value);
                    }
                    else
                    {
                        currentAsyncLocalBoxedContextValue.Value = value;
                    }
                }
            }
            else if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
            {
                s_currentThreadStaticContext = value;
            }
            else
            {
                s_currentStaticContext = value;
            }
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
    public static AtataContext? Global { get; internal set; }

    [Obsolete("Use BaseConfiguration instead.")] // Obsolete since v4.0.0.
    public static AtataContextBuilder GlobalConfiguration => BaseConfiguration;

    /// <summary>
    /// Gets the base configuration builder.
    /// </summary>
    public static AtataContextBuilder BaseConfiguration { get; internal set; } =
        new(contextScope: null, sessionStartScopes: AtataContextScopes.None);

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
    public AtataContext? ParentContext { get; }

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
    /// Gets the test result status.
    /// </summary>
    public TestResultStatus TestResultStatus { get; internal set; }

    /// <summary>
    /// Gets the execution unit.
    /// </summary>
    public IAtataExecutionUnit ExecutionUnit { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is active (not disposed).
    /// </summary>
    public bool IsActive =>
        _status == Status.Active;

    /// <summary>
    /// Gets the unique context identifier.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets a collection of sessions associated with the context.
    /// </summary>
    public AtataSessionCollection Sessions { get; }

    [Obsolete("Use Sessions.Get<WebDriverSession>().DriverFactory instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal IWebDriverFactory? DriverFactory { get; set; }

    [Obsolete("Use Sessions.Get<WebDriverSession>().Driver instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IWebDriver Driver =>
        Sessions.Get<WebDriverSession>().Driver;

    [Obsolete("Use Sessions.Contains<WebDriverSession>() instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasDriver =>
        Sessions.Contains<WebDriverSession>();

    [Obsolete("Use Sessions.Get<WebDriverSession>().DriverAlias instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string? DriverAlias =>
        Sessions.Get<WebDriverSession>().DriverAlias;

    /// <summary>
    /// Gets the instance of the log manager.
    /// </summary>
    public ILogManager Log { get; internal set; } = null!;

    /// <summary>
    /// Gets the local date/time of the start.
    /// </summary>
    public DateTime StartedAt { get; }

    /// <summary>
    /// Gets the UTC date/time of the start.
    /// </summary>
    public DateTime StartedAtUtc { get; }

    [Obsolete("Use Sessions.Get<WebDriverSession>().BaseUrl instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string? BaseUrl =>
        Sessions.Get<WebDriverSession>().BaseUrl;

    /// <summary>
    /// Gets the base retry timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan BaseRetryTimeout { get; internal set; }

    /// <summary>
    /// Gets the base retry interval.
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    public TimeSpan BaseRetryInterval { get; internal set; }

    [Obsolete("Use Sessions.Get<WebDriverSession>().ElementFindTimeout instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TimeSpan ElementFindTimeout =>
        Sessions.Get<WebDriverSession>().ElementFindTimeout;

    [Obsolete("Use Sessions.Get<WebDriverSession>().ElementFindRetryInterval instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TimeSpan ElementFindRetryInterval =>
        Sessions.Get<WebDriverSession>().ElementFindRetryInterval;

    /// <summary>
    /// Gets the waiting timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan WaitingTimeout { get; internal set; }

    /// <summary>
    /// Gets the waiting retry interval.
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    public TimeSpan WaitingRetryInterval { get; internal set; }

    /// <summary>
    /// Gets the verification timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan VerificationTimeout { get; internal set; }

    /// <summary>
    /// Gets the verification retry interval.
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    public TimeSpan VerificationRetryInterval { get; internal set; }

    [Obsolete("Use Sessions.Get<WebDriverSession>().DefaultControlVisibility instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Visibility DefaultControlVisibility =>
        Sessions.Get<WebDriverSession>().DefaultControlVisibility;

    /// <summary>
    /// Gets the default cancellation token.
    /// </summary>
    public CancellationToken DefaultCancellationToken { get; internal set; }

    /// <summary>
    /// Gets the culture.
    /// The default value is <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    public CultureInfo Culture { get; internal set; } = null!;

    /// <summary>
    /// Gets the assertion exception factory.
    /// The default value is an instance of <see cref="AtataAssertionExceptionFactory"/>.
    /// </summary>
    public IAssertionExceptionFactory AssertionExceptionFactory { get; internal set; } = null!;

    /// <summary>
    /// Gets the aggregate assertion exception factory.
    /// The default value is an instance of <see cref="AtataAggregateAssertionExceptionFactory"/>.
    /// </summary>
    public IAggregateAssertionExceptionFactory AggregateAssertionExceptionFactory { get; internal set; } = null!;

    /// <summary>
    /// Gets the aggregate assertion strategy.
    /// The default value is an instance of <see cref="AtataAggregateAssertionStrategy"/>.
    /// </summary>
    public IAggregateAssertionStrategy AggregateAssertionStrategy { get; internal set; } = null!;

    /// <summary>
    /// Gets the aggregate assertion depth level.
    /// </summary>
    public int AggregateAssertionLevel { get; internal set; }

    /// <summary>
    /// Gets the strategy for warning assertion reporting.
    /// The default value is an instance of <see cref="AtataWarningReportStrategy"/>.
    /// </summary>
    public IWarningReportStrategy WarningReportStrategy { get; internal set; } = null!;

    /// <summary>
    /// Gets the strategy for assertion failure reporting.
    /// The default value is an instance of <see cref="AtataAssertionFailureReportStrategy"/>.
    /// </summary>
    public IAssertionFailureReportStrategy AssertionFailureReportStrategy { get; internal set; } = null!;

    /// <summary>
    /// Gets the list of all assertion results.
    /// </summary>
    public List<AssertionResult> AssertionResults { get; } = [];

    /// <summary>
    /// Gets the list of pending assertion results with <see cref="AssertionStatus.Failed"/> or <see cref="AssertionStatus.Warning"/> status.
    /// </summary>
    public List<AssertionResult> PendingFailureAssertionResults { get; } = [];

    internal Exception? LastLoggedException { get; set; }

    /// <summary>
    /// Gets the context of the attributes.
    /// </summary>
    public AtataAttributesContext Attributes { get; internal set; } = null!;

    /// <summary>
    /// Gets the <see cref="DirectorySubject"/> of Artifacts directory.
    /// Artifacts directory can contain any files produced during test execution, logs, screenshots, downloads, etc.
    /// The default Artifacts directory path is <c>"{test-suite-name-sanitized:/*}{test-name-sanitized:/*}"</c>
    /// relative to <see cref="AtataContextGlobalProperties.ArtifactsRootPath"/> value
    /// of <see cref="GlobalProperties"/>.
    /// </summary>
    public DirectorySubject Artifacts { get; private set; } = null!;

    /// <summary>
    /// Gets the full path of Artifacts directory.
    /// Artifacts directory can contain any files produced during test execution, logs, screenshots, downloads, etc.
    /// The default typical Artifacts full path is <c>"SomeRelativeNamespace/SomeTestClass/SomeTest"</c>
    /// appended to <see cref="AtataContextGlobalProperties.ArtifactsRootPath"/> value
    /// of <see cref="GlobalProperties"/>.
    /// </summary>
    public string ArtifactsPath =>
        Artifacts.FullName.Value;

    /// <summary>
    /// Gets the relative path of Artifacts directory.
    /// Artifacts directory can contain any files produced during test execution, logs, screenshots, downloads, etc.
    /// The default typical Artifacts relative path is <c>"SomeRelativeNamespace/SomeTestClass/SomeTest"</c>.
    /// </summary>
    public string ArtifactsRelativePath { get; private set; } = null!;

    [Obsolete("Use Sessions.Get<WebDriverSession>().Go instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public WebDriverSessionNavigator Go =>
        Sessions.Get<WebDriverSession>().Go;

    /// <summary>
    /// Gets the <see cref="IReport{TOwner}"/> instance that provides a reporting functionality.
    /// </summary>
    public IReport<AtataContext> Report { get; }

    [Obsolete("Use Sessions.Get<WebDriverSession>().PageObject instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public UIComponent? PageObject =>
        Sessions.Get<WebDriverSession>().PageObject;

    internal Stopwatch ExecutionStopwatch { get; } = Stopwatch.StartNew();

    internal Stopwatch BodyExecutionStopwatch { get; } = new();

    internal Stopwatch SetupExecutionStopwatch { get; } = new();

    [Obsolete("Use Sessions.Get<WebDriverSession>().TemporarilyPreservedPageObjects instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IReadOnlyList<UIComponent> TemporarilyPreservedPageObjects =>
        Sessions.Get<WebDriverSession>().TemporarilyPreservedPageObjects;

    [Obsolete("Use Sessions.Get<WebDriverSession>().UIComponentAccessChainScopeCache instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public UIComponentAccessChainScopeCache UIComponentAccessChainScopeCache =>
        Sessions.Get<WebDriverSession>().UIComponentAccessChainScopeCache;

    /// <summary>
    /// Gets the event bus of <see cref="AtataContext"/>,
    /// which can used to subscribe to and publish events.
    /// </summary>
    public IEventBus EventBus { get; internal set; } = null!;

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

    [Obsolete("Use Sessions.Get<WebDriverSession>().DomTestIdAttributeName instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string DomTestIdAttributeName =>
        Sessions.Get<WebDriverSession>().DomTestIdAttributeName;

    [Obsolete("Use Sessions.Get<WebDriverSession>().DomTestIdAttributeDefaultCase instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TermCase DomTestIdAttributeDefaultCase =>
        Sessions.Get<WebDriverSession>().DomTestIdAttributeDefaultCase;

    /// <summary>
    /// Gets the current <see cref="AtataContext"/> instance.
    /// If it's missing (<see cref="Current"/> is <see langword="null"/>), throws <see cref="AtataContextNotFoundException"/>.
    /// </summary>
    /// <returns>An <see cref="AtataContext"/> instance.</returns>
    public static AtataContext ResolveCurrent() =>
        Current ?? throw AtataContextNotFoundException.ForCurrentIsNull();

    /// <summary>
    /// Presets the current asynchronous local box when current mode is <see cref="AtataContextModeOfCurrent.AsyncLocalBoxed"/>.
    /// </summary>
    public static void PresetCurrentAsyncLocalBox()
    {
        if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocalBoxed)
        {
            s_currentAsyncLocalBoxedContext.Value = new();
        }
    }

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

    internal void Activate() =>
        _status = Status.Active;

    internal void AddChildContext(AtataContext context) =>
        _childContexts.Add(context);

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

    internal void InitCustomVariables(IEnumerable<KeyValuePair<string, object?>> customVariables)
    {
        var variables = Variables;

        foreach (var variable in customVariables)
            variables.SetInitialValue(variable.Key, variable.Value);
    }

    internal void InitState(IEnumerable<KeyValuePair<string, object?>> stateObjects)
    {
        var state = State;

        foreach (var stateObject in stateObjects)
            state.SetInitialValue(stateObject.Key, stateObject.Value);
    }

    internal void InitArtifactsDirectory()
    {
        string relativePath = CreateArtifactsRelativePath();
        string fullPath = Path.Combine(GlobalProperties.ArtifactsRootPath, relativePath);

        ArtifactsRelativePath = relativePath;
        Artifacts = new(fullPath, "Artifacts", ExecutionUnit);
    }

    private string CreateArtifactsRelativePath()
    {
        string relativePath = GlobalProperties.ArtifactsPathFactory.Create(this);

        if (relativePath is null or [])
            return string.Empty;

        return relativePath[0] == Path.DirectorySeparatorChar || relativePath[0] == Path.AltDirectorySeparatorChar
            ? relativePath[1..]
            : relativePath;
    }

    internal void InitArtifactsVariable() =>
        Variables.SetInitialValue("artifacts", ArtifactsPath);

    internal void LogTestStart()
    {
        var logMessageBuilder = new StringBuilder($"Starting ")
            .Append(GetScopeNameForLog());

        if (!Test.IsEmpty)
            logMessageBuilder.Append(' ').Append(Test);

        logMessageBuilder.Append(" at ")
            .Append(ConvertDateTimeToString(StartedAt));

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
    public void AggregateAssert(Action action, string? assertionScopeName = null) =>
        AggregateAssert(action, Log, assertionScopeName);

    internal void AggregateAssert(Action action, ILogManager log, string? assertionScopeName = null)
    {
        Guard.ThrowIfNull(action);

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

    internal void EnsureExceptionIsLogged(Exception exception, ILogManager? log = null)
    {
        if (exception != LastLoggedException)
        {
            LastLoggedException = exception;

            (log ?? Log).Error(exception);
            TakeFailureSnapshot();
        }
    }

    [Obsolete("Use Sessions.Get<WebDriverSession>().RestartDriver() instead.")] // Obsolete since v4.0.0.
    public void RestartDriver() =>
        Sessions.Get<WebDriverSession>().RestartDriver();

    [Obsolete("Use Sessions.Get<WebDriverSession>().TakeScreenshot(string) instead.")] // Obsolete since v4.0.0.
    public void TakeScreenshot(string? title = null) =>
        Sessions.Get<WebDriverSession>().TakeScreenshot(title);

    [Obsolete("Use Sessions.Get<WebDriverSession>().TakeScreenshot(ScreenshotKind, string) instead.")] // Obsolete since v4.0.0.
    public void TakeScreenshot(ScreenshotKind kind, string? title = null) =>
        Sessions.Get<WebDriverSession>().TakeScreenshot(kind, title);

    [Obsolete("Use Sessions.Get<WebDriverSession>().TakePageSnapshot(string) instead.")] // Obsolete since v4.0.0.
    public void TakePageSnapshot(string? title = null) =>
        Sessions.Get<WebDriverSession>().TakePageSnapshot(title);

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePathWithoutExtension">The relative file path without extension.</param>
    /// <param name="fileContentWithExtension">The file content with extension.</param>
    /// <param name="options">The artifact adding options.</param>
    /// <returns>A <see cref="FileSubject"/> for added file.</returns>
    public FileSubject AddArtifact(string relativeFilePathWithoutExtension, FileContentWithExtension fileContentWithExtension, in AddArtifactOptions options = default)
    {
        if (!options.PrependArtifactNumberToFileName)
            Guard.ThrowIfNullOrWhitespace(relativeFilePathWithoutExtension);
        Guard.ThrowIfNull(fileContentWithExtension);

        string relativeFilePath = relativeFilePathWithoutExtension + fileContentWithExtension.Extension;

        return DoAddArtifact(
            relativeFilePath,
            options,
            fileContentWithExtension.Save);
    }

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePath">The relative file path.</param>
    /// <param name="fileBytes">The file bytes.</param>
    /// <param name="options">The artifact adding options.</param>
    /// <returns>A <see cref="FileSubject"/> for added file.</returns>
    public FileSubject AddArtifact(string relativeFilePath, byte[] fileBytes, in AddArtifactOptions options = default)
    {
        if (!options.PrependArtifactNumberToFileName)
            Guard.ThrowIfNullOrWhitespace(relativeFilePath);
        Guard.ThrowIfNull(fileBytes);

        return DoAddArtifact(
            relativeFilePath,
            options,
            absoluteFilePath =>
                File.WriteAllBytes(absoluteFilePath, fileBytes));
    }

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePath">The relative file path.</param>
    /// <param name="fileContent">Content of the file.</param>
    /// <param name="options">The artifact adding options.</param>
    /// <returns>A <see cref="FileSubject"/> for added file.</returns>
    public FileSubject AddArtifact(string relativeFilePath, string fileContent, in AddArtifactOptions options = default)
    {
        if (!options.PrependArtifactNumberToFileName)
            Guard.ThrowIfNullOrWhitespace(relativeFilePath);
        Guard.ThrowIfNull(fileContent);

        return DoAddArtifact(
            relativeFilePath,
            options,
            absoluteFilePath =>
                File.WriteAllText(absoluteFilePath, fileContent));
    }

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePath">The relative file path.</param>
    /// <param name="fileContent">Content of the file.</param>
    /// <param name="encoding">The encoding. Can be <see langword="null"/>.</param>
    /// <param name="options">The artifact adding options.</param>
    /// <returns>A <see cref="FileSubject"/> for added file.</returns>
    public FileSubject AddArtifact(string relativeFilePath, string fileContent, Encoding encoding, in AddArtifactOptions options = default)
    {
        if (!options.PrependArtifactNumberToFileName)
            Guard.ThrowIfNullOrWhitespace(relativeFilePath);
        Guard.ThrowIfNull(fileContent);

        return DoAddArtifact(
            relativeFilePath,
            options,
            absoluteFilePath =>
            {
                if (encoding is null)
                    File.WriteAllText(absoluteFilePath, fileContent);
                else
                    File.WriteAllText(absoluteFilePath, fileContent, encoding);
            });
    }

    /// <summary>
    /// Adds the file to the Artifacts directory.
    /// </summary>
    /// <param name="relativeFilePath">The relative file path.</param>
    /// <param name="stream">The stream to write to the file.</param>
    /// <param name="options">The artifact adding options.</param>
    /// <returns>A <see cref="FileSubject"/> for added file.</returns>
    public FileSubject AddArtifact(string relativeFilePath, Stream stream, in AddArtifactOptions options = default)
    {
        if (!options.PrependArtifactNumberToFileName)
            Guard.ThrowIfNullOrWhitespace(relativeFilePath);
        Guard.ThrowIfNull(stream);

        return DoAddArtifact(
            relativeFilePath,
            options,
            absoluteFilePath =>
            {
                using FileStream source = File.Create(absoluteFilePath);
                stream.CopyTo(source);
            });
    }

    private FileSubject DoAddArtifact(string relativeFilePath, in AddArtifactOptions options, Action<string> fileSaveAction)
    {
        if (options.PrependArtifactNumberToFileName)
        {
            int currentArtifactNumber = Interlocked.Increment(ref _artifactNumber);
            string currentArtifactNumberString = currentArtifactNumber.ToString("D3", CultureInfo.InvariantCulture);

            relativeFilePath = relativeFilePath?.Length > 0
                ? relativeFilePath[0] != '.'
                    ? $"{currentArtifactNumberString}-{relativeFilePath}"
                    : $"{currentArtifactNumberString}{relativeFilePath}"
                : currentArtifactNumberString;
        }

        string absoluteFilePath = BuildAbsoluteArtifactFilePathAndEnsureDirectoryExists(relativeFilePath);

        fileSaveAction.Invoke(absoluteFilePath);

        EventBus.Publish(new ArtifactAddedEvent(absoluteFilePath, relativeFilePath, options.ArtifactType, options.ArtifactTitle));

        return Artifacts.Files[relativeFilePath];
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
    /// Sets the test result status as <see cref="TestResultStatus.Inconclusive"/>
    /// and optionally logs the inconclusive message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void SetInconclusiveTestResult(string? message = null)
    {
        EnsureNotDisposed();

        TestResultStatus = TestResultStatus.Inconclusive;

        if (message?.Length > 0)
            Log.Info(message);
    }

    /// <summary>
    /// Handles the test result exception.
    /// Sets the test result status as <see cref="TestResultStatus.Failed"/>.
    /// Logs the error and executes snapshots taking functionality for active sessions.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public void HandleTestResultException(Exception exception)
    {
        Guard.ThrowIfNull(exception);

        EnsureNotDisposed();

        TestResultStatus = TestResultStatus.Failed;

        if (exception != LastLoggedException)
        {
            Log.Error(exception);

            TakeFailureSnapshot();
        }
    }

    /// <summary>
    /// Handles the test result exception.
    /// Sets the test result status as <see cref="TestResultStatus.Failed"/>.
    /// Logs the error and executes snapshots taking functionality for active sessions.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="stackTrace">The exception stack trace.</param>
    public void HandleTestResultException(string message, string? stackTrace)
    {
        Guard.ThrowIfNull(message);

        EnsureNotDisposed();

        TestResultStatus = TestResultStatus.Failed;

        if (LastLoggedException is null || !message.Contains(LastLoggedException.Message))
        {
            string completeErrorMessage = VerificationUtils.AppendStackTraceToFailureMessage(message, stackTrace);
            Log.Error(completeErrorMessage);

            TakeFailureSnapshot();
        }
    }

    internal void TakeFailureSnapshot()
    {
        foreach (AtataSession session in Sessions)
        {
            if (session.IsActive)
                session.TakeFailureSnapshot();
        }
    }

    [Obsolete("Use RaiseAssertionError(...) instead.")] // Obsolete since v4.0.0.
    public void RaiseError(string message, Exception? exception = null) =>
        RaiseAssertionError(message, exception);

    /// <summary>
    /// Raises the error by throwing an assertion exception.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The optional exception.</param>
    public void RaiseAssertionError(string message, Exception? exception = null) =>
        AssertionVerificationStrategy.Instance.ReportFailure(ExecutionUnit, message, exception);

    [Obsolete("Use RaiseAssertionWarning(...) instead.")] // Obsolete since v4.0.0.
    public void RaiseWarning(string message, Exception? exception = null) =>
        RaiseAssertionWarning(message, exception);

    /// <summary>
    /// Raises the warning by recording an assertion warning.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The optional exception.</param>
    public void RaiseAssertionWarning(string message, Exception? exception = null) =>
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
        else if (GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocalBoxed)
        {
            var currentAsyncLocalBoxedContextValue = s_currentAsyncLocalBoxedContext.Value;

            if (currentAsyncLocalBoxedContextValue?.Value != this)
            {
                if (currentAsyncLocalBoxedContextValue is null)
                {
                    s_currentAsyncLocalBoxedContext.Value = new(this);
                }
                else
                {
                    currentAsyncLocalBoxedContextValue.Value = this;
                }
            }
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

    [Obsolete("Use Variables.FillTemplateString(...) instead.")] // Obsolete since v4.0.0.
    public string FillTemplateString(string template) =>
        Variables.FillTemplateString(template);

    [Obsolete("Use Variables.FillTemplateString(...) instead.")] // Obsolete since v4.0.0.
    public string FillTemplateString(string template, IEnumerable<KeyValuePair<string, object?>>? additionalVariables) =>
        Variables.FillTemplateString(template, additionalVariables);

    [Obsolete("Use Variables.FillPathTemplateString(...) instead.")] // Obsolete since v4.0.0.
    public string FillPathTemplateString(string template) =>
        Variables.FillPathTemplateString(template);

    [Obsolete("Use Variables.FillPathTemplateString(...) instead.")] // Obsolete since v4.0.0.
    public string FillPathTemplateString(string template, IEnumerable<KeyValuePair<string, object?>>? additionalVariables) =>
        Variables.FillPathTemplateString(template, additionalVariables);

    [Obsolete("Use Variables.FillUriTemplateString(...) instead.")] // Obsolete since v4.0.0.
    public string FillUriTemplateString(string template) =>
        Variables.FillUriTemplateString(template);

    [Obsolete("Use Variables.FillUriTemplateString(...) instead.")] // Obsolete since v4.0.0.
    public string FillUriTemplateString(string template, IEnumerable<KeyValuePair<string, object?>>? additionalVariables) =>
        Variables.FillUriTemplateString(template, additionalVariables);

    public void SetToDefaultCancellationTokenWhenDefault(ref CancellationToken cancellationToken)
    {
        if (cancellationToken == default)
            cancellationToken = DefaultCancellationToken;
    }

    /// <summary>
    /// <para>
    /// Deinitializes and disposes the current context together with sessions and other associated objects.
    /// Also writes the execution time to log.
    /// Throws <see cref="AggregateAssertionException"/> if
    /// <see cref="PendingFailureAssertionResults"/> is not empty (contains warnings).
    /// </para>
    /// <para>
    /// Publishes events:
    /// <list type="number">
    /// <item><see cref="AtataContextDeInitStartedEvent"/></item>
    /// <item><see cref="AtataSessionDeInitStartedEvent"/> for sessions</item>
    /// <item><see cref="AtataSessionDeInitCompletedEvent"/> for sessions</item>
    /// <item><see cref="AtataContextDeInitCompletedEvent"/></item>
    /// </list>
    /// </para>
    /// </summary>
    public void Dispose()
    {
        if (_status == Status.Disposed)
            return;

        var exceptions = SafelyDisposeAsync().RunSync();

        if (Current == this)
            Current = null;

        ThrowPendingExceptions(exceptions);
    }

    /// <inheritdoc cref="Dispose"/>
    /// <returns>A <see cref="ValueTask"/> object.</returns>
    public async ValueTask DisposeAsync()
    {
        if (_status == Status.Disposed)
            return;

        var exceptions = await SafelyDisposeAsync().ConfigureAwait(false);

        if (Current == this)
            Current = null;

        ThrowPendingExceptions(exceptions);
    }

    private async Task<List<Exception>> SafelyDisposeAsync()
    {
        BodyExecutionStopwatch.Stop();
        Stopwatch deinitializationStopwatch = Stopwatch.StartNew();

        List<Exception> exceptions = [];

        await Log.ExecuteSectionAsync(
            new LogSection($"Deinitialize {ToShortString()}", LogLevel.Trace),
            async () =>
            {
                try
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    await EventBus.PublishAsync(new AtataContextDeInitEvent(this))
                        .ConfigureAwait(false);
#pragma warning restore CS0618 // Type or member is obsolete

                    await EventBus.PublishAsync(new AtataContextDeInitStartedEvent(this))
                        .ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    exceptions.Add(exception);
                }

                UpdateTestResultStatusBasedOnChildContexts();

                // A session during Dispose is removed from Sessions, so ToArray() is used.
                var sessions = Sessions.GetAllIncludingPooled().ToArray();

                foreach (var session in sessions)
                {
                    try
                    {
                        if (session.IsBorrowedOrTakenFromPool && session.OwnerContext != this)
                            session.ReturnToSessionSource();
                        else
                            await session.DisposeAsync().ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(exception);
                    }
                }

                Sessions.Dispose();

                try
                {
                    await EventBus.PublishAsync(new AtataContextDeInitCompletedEvent(this))
                        .ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    exceptions.Add(exception);
                }

                EventBus.UnsubscribeAll();
                State.Clear();
            })
            .ConfigureAwait(false);

        deinitializationStopwatch.Stop();

        LogTestFinish(deinitializationStopwatch.Elapsed);

        Variables.Clear();
        Log = null!;
        AssertionResults.Clear();

        _status = Status.Disposed;
        return exceptions;
    }

    private void UpdateTestResultStatusBasedOnChildContexts()
    {
        const TestResultStatus maxStatus = TestResultStatus.Failed;

        if (TestResultStatus == maxStatus)
            return;

        if (TestResultStatus == TestResultStatus.None)
            TestResultStatus = TestResultStatus.Passed;

        for (int i = 0; i < _childContexts.Count; i++)
        {
            var status = _childContexts[i].TestResultStatus;
            if (status > TestResultStatus)
            {
                TestResultStatus = status;

                if (status == maxStatus)
                    return;
            }
        }
    }

    private void ThrowPendingExceptions(List<Exception> disposeExceptions)
    {
        if (PendingFailureAssertionResults.Count > 0)
        {
            var pendingFailureAssertionResults = GetAndClearPendingFailureAssertionResults();
            var aggregateAssertionException = VerificationUtils.CreateAggregateAssertionException(this, pendingFailureAssertionResults);
            disposeExceptions.Insert(0, aggregateAssertionException);
        }

        if (disposeExceptions.Count > 1)
        {
            throw new AggregateException("Multiple errors.", disposeExceptions);
        }
        else if (disposeExceptions.Count == 1)
        {
            throw disposeExceptions[0];
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

        string scopeName = GetScopeNameForLog();

        DateTime finishedAt = GlobalProperties.ConvertToTimeZone(DateTime.UtcNow);

        StringBuilder messageBuilder = new(
            $"""
            Finished {scopeName} with {GetTestResultStatusForLog()} status at {ConvertDateTimeToString(finishedAt)}
                  Total time: {totalTimeString.PadLeft(maxTimeStringLength)}
              Initialization: {initializationTimeString.PadLeft(maxTimeStringLength)} | {initializationTimePercentString.PadLeft(maxPercentStringLength)}
            """);

        if (setupTime > TimeSpan.Zero)
            messageBuilder.AppendLine().Append(
                $"           Setup: {setupTimeString.PadLeft(maxTimeStringLength)} | {setupTimePercentString.PadLeft(maxPercentStringLength)}");

        string simplifiedScopeName = Scope switch
        {
            AtataContextScope.Test => "Test",
            null => "Unit",
            _ => "Suite",
        };

        messageBuilder.AppendLine().Append(
            $"""
            {$"{simplifiedScopeName} body:",17} {testBodyTimeString.PadLeft(maxTimeStringLength)} | {testBodyPercentString.PadLeft(maxPercentStringLength)}
            Deinitialization: {deinitializationTimeString.PadLeft(maxTimeStringLength)} | {deinitializationTimePercentString.PadLeft(maxPercentStringLength)}
            """);

        Log.Debug(messageBuilder.ToString());
    }

    private string GetScopeNameForLog() =>
        Scope switch
        {
            AtataContextScope.Global => "global suite",
            AtataContextScope.Namespace => "namespace suite",
            AtataContextScope.TestSuiteGroup => "test suite group",
            AtataContextScope.TestSuite => "test suite",
            AtataContextScope.Test => "test",
            _ => "unit"
        };

    private string GetTestResultStatusForLog() =>
        TestResultStatus switch
        {
            TestResultStatus.None => "none",
            TestResultStatus.Inconclusive => "inconclusive",
            TestResultStatus.Passed => "passed",
            TestResultStatus.Warning => "warning",
            TestResultStatus.Failed => "failed",
            _ => "unknown"
        };

    private void EnsureNotDisposed()
    {
        if (_status == Status.Disposed)
            throw new ObjectDisposedException(GetType().FullName);
    }

    /// <inheritdoc/>
    public override string ToString() =>
        _lazyStringRepresentation.Value;

    internal string ToShortString() =>
        _lazyShortStringRepresentation.Value;

    private static string ConvertDateTimeToString(DateTime dateTime) =>
        dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

    private string ToStringCore()
    {
        var builder = new StringBuilder("AtataContext { Id=")
            .Append(Id);

        if (!Test.IsEmpty)
            builder.Append(", Test=")
                .Append(Test);

        builder.Append(" }");

        return builder.ToString();
    }

    private string ToShortStringCore() =>
        $"AtataContext {{ Id={Id} }}";
}
