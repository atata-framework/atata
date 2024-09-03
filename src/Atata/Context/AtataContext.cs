using Atata.Context;

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

    private readonly AssertionVerificationStrategy _assertionVerificationStrategy;

    private readonly ExpectationVerificationStrategy _expectationVerificationStrategy;

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
        : this(null)
    {
    }

    internal AtataContext(AtataContext parentContext)
    {
        ParentContext = parentContext;

        _assertionVerificationStrategy = new AssertionVerificationStrategy(this);
        _expectationVerificationStrategy = new ExpectationVerificationStrategy(this);

        Report = new Report<AtataContext>(this, this);

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
    /// Gets the global configuration.
    /// </summary>
    public static AtataContextBuilder GlobalConfiguration { get; } = new AtataContextBuilder(new AtataBuildingContext());

    public AtataContext ParentContext { get; }

    public AtataSessionCollection Sessions { get; } = [];

    [Obsolete("Use GetWebDriverSession().DriverFactory instead.")] // Obsolete since v4.0.0.
    internal IWebDriverFactory DriverFactory { get; set; }

    [Obsolete("Use GetWebDriver() or GetWebDriverSession().Driver instead.")] // Obsolete since v4.0.0.
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IWebDriver Driver =>
        this.GetWebDriverSession().Driver;

    // TODO: Change HasDriver obsolete message.
    [Obsolete("Use GetWebDriverSession().HasDriver instead.")] // Obsolete since v4.0.0.
    public bool HasDriver =>
        this.GetWebDriverSession().HasDriver;

    [Obsolete("Use GetWebDriverSession().DriverAlias instead.")] // Obsolete since v4.0.0.
    public string DriverAlias =>
        this.GetWebDriverSession().DriverAlias;

    /// <summary>
    /// Gets the instance of the log manager.
    /// </summary>
    public ILogManager Log { get; internal set; }

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

    [Obsolete("Use GetWebSession().BaseUrl instead.")] // Obsolete since v4.0.0.
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
    public TimeSpan ElementFindTimeout =>
        this.GetWebSession().ElementFindTimeout;

    [Obsolete("Use GetWebSession().ElementFindRetryInterval instead.")] // Obsolete since v4.0.0.
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
    public AtataNavigator Go =>
        this.GetWebSession().Go;

    /// <summary>
    /// Gets the <see cref="Report{TOwner}"/> instance that provides a reporting functionality.
    /// </summary>
    public Report<AtataContext> Report { get; }

    [Obsolete("Use GetWebSession().PageObject instead.")] // Obsolete since v4.0.0.
    public UIComponent PageObject =>
        this.GetWebSession().PageObject;

    internal Stopwatch ExecutionStopwatch { get; } = Stopwatch.StartNew();

    internal Stopwatch BodyExecutionStopwatch { get; } = new Stopwatch();

    internal Stopwatch SetupExecutionStopwatch { get; } = new Stopwatch();

    [Obsolete("Use GetWebDriverSession().TemporarilyPreservedPageObjects instead.")] // Obsolete since v4.0.0.
    public IReadOnlyList<UIComponent> TemporarilyPreservedPageObjects =>
        this.GetWebDriverSession().TemporarilyPreservedPageObjects;

    [Obsolete("Use GetWebDriverSession().UIComponentAccessChainScopeCache instead.")] // Obsolete since v4.0.0.
    public UIComponentAccessChainScopeCache UIComponentAccessChainScopeCache =>
        this.GetWebDriverSession().UIComponentAccessChainScopeCache;

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
    public string DomTestIdAttributeName =>
        this.GetWebSession().DomTestIdAttributeName;

    [Obsolete("Use GetWebSession().DomTestIdAttributeDefaultCase instead.")] // Obsolete since v4.0.0.
    public TermCase DomTestIdAttributeDefaultCase =>
        this.GetWebSession().DomTestIdAttributeDefaultCase;

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
        StartedAt = TimeZoneInfo.ConvertTimeFromUtc(StartedAtUtc, GlobalProperties.TimeZone);
    }

    internal void InitMainVariables()
    {
        var variables = Variables;

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
        StringBuilder logMessageBuilder = new StringBuilder(
            $"Starting {Test.GetTestUnitKindName()}");

        string testFullName = Test.FullName;

        if (testFullName is not null)
            logMessageBuilder.Append(": ").Append(testFullName);

        Log.Debug(logMessageBuilder.ToString());
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

        try
        {
            AggregateAssertionStrategy.Assert(() =>
            {
                AggregateAssertionLevel++;

                try
                {
                    Log.ExecuteSection(
                        new AggregateAssertionLogSection(assertionScopeName),
                        () =>
                        {
                            try
                            {
                                action.Invoke();
                            }
                            catch (Exception exception)
                            {
                                EnsureExceptionIsLogged(exception);
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

    internal void EnsureExceptionIsLogged(Exception exception)
    {
        if (exception != LastLoggedException)
        {
            Log.Error(exception.ToString());
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
    /// If <see cref="AtataBuildingContext.DisposeDriver"/> property is set to <see langword="true"/> (by default),
    /// then the <see cref="Driver"/> will also be disposed.
    /// Publishes events: <see cref="AtataContextDeInitEvent"/>, <see cref="DriverDeInitEvent"/>, <see cref="AtataContextDeInitCompletedEvent"/>.
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

                // TODO: Dispose sessions, which are needed to be disposed.

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
            var pendingFailureAssertionResults = GetAndClearPendingFailureAssertionResults();
            throw VerificationUtils.CreateAggregateAssertionException(pendingFailureAssertionResults);
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
}
