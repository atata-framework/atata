using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the Atata context, the entry point for the test set-up.
    /// </summary>
    public sealed class AtataContext : IDisposable
    {
        private static readonly object s_buildStartSyncLock = new object();

        private static readonly AsyncLocal<AtataContext> s_currentAsyncLocalContext = new AsyncLocal<AtataContext>();

        private static AtataContextModeOfCurrent s_modeOfCurrent = AtataContextModeOfCurrent.AsyncLocal;

        [ThreadStatic]
        private static AtataContext s_currentThreadStaticContext;

        private static AtataContext s_currentStaticContext;

        private string _testName;

        private string _testSuiteName;

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
            Go = new AtataNavigator(this);
            Report = new Report<AtataContext>(this, this);
        }

        /// <summary>
        /// Gets or sets the current context.
        /// </summary>
        public static AtataContext Current
        {
            get =>
                ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic
                    ? s_currentThreadStaticContext
                    : ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal
                    ? s_currentAsyncLocalContext.Value
                    : s_currentStaticContext;

            set
            {
                if (ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
                    s_currentThreadStaticContext = value;
                else if (ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
                    s_currentAsyncLocalContext.Value = value;
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

                RetrySettings.ThreadBoundary = value == AtataContextModeOfCurrent.ThreadStatic
                    ? RetrySettingsThreadBoundary.ThreadStatic
                    : value == AtataContextModeOfCurrent.AsyncLocal
                        ? RetrySettingsThreadBoundary.AsyncLocal
                        : RetrySettingsThreadBoundary.Static;
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

        /// <summary>
        /// Gets the driver initialization stage.
        /// </summary>
        public AtataContextDriverInitializationStage DriverInitializationStage { get; internal set; }

        /// <summary>
        /// Gets the instance of the log manager.
        /// </summary>
        public ILogManager Log { get; internal set; }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public string TestName
        {
            get => _testName;
            internal set
            {
                _testName = value;
                TestNameSanitized = value.SanitizeForFileName();
            }
        }

        /// <summary>
        /// Gets the name of the test sanitized for file path/name.
        /// </summary>
        public string TestNameSanitized { get; private set; }

        /// <summary>
        /// Gets the name of the test suite (fixture/class).
        /// </summary>
        public string TestSuiteName
        {
            get => _testSuiteName;
            internal set
            {
                _testSuiteName = value;
                TestSuiteNameSanitized = value.SanitizeForFileName();
            }
        }

        /// <summary>
        /// Gets the name of the test suite sanitized for file path/name.
        /// </summary>
        public string TestSuiteNameSanitized { get; private set; }

        /// <summary>
        /// Gets the test suite (fixture/class) type.
        /// </summary>
        public Type TestSuiteType { get; internal set; }

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

        internal Stopwatch PureExecutionStopwatch { get; } = new Stopwatch();

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

            variables["test-name-sanitized"] = TestNameSanitized;
            variables["test-name"] = TestName;
            variables["test-suite-name-sanitized"] = TestSuiteNameSanitized;
            variables["test-suite-name"] = TestSuiteName;
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
                $"Starting {GetTestUnitKindName()}");

            string[] testFullNameParts = GetTestFullNameParts().ToArray();

            if (testFullNameParts.Length > 0)
            {
                logMessageBuilder.Append(": ")
                    .Append(string.Join(".", testFullNameParts));
            }

            Log.Info(logMessageBuilder.ToString());
        }

        private IEnumerable<string> GetTestFullNameParts()
        {
            if (TestSuiteType != null)
                yield return TestSuiteType.Namespace;

            if (TestSuiteName != null)
                yield return TestSuiteName;

            if (TestName != null)
                yield return TestName;
        }

        private string GetTestUnitKindName() =>
            TestName != null
                ? "test"
                : TestSuiteType != null
                    ? "test suite"
                    : "test unit";

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

        /// <summary>
        /// Cleans up the test context.
        /// </summary>
        /// <param name="quitDriver">if set to <see langword="true"/> quits WebDriver.</param>
        public void CleanUp(bool quitDriver = true)
        {
            if (_disposed)
                return;

            PureExecutionStopwatch.Stop();

            Log.ExecuteSection(
                new LogSection("Clean up AtataContext", LogLevel.Trace),
                () =>
                {
                    EventBus.Publish(new AtataContextCleanUpEvent(this));

                    CleanUpTemporarilyPreservedPageObjectList();

                    if (PageObject != null)
                        UIComponentResolver.CleanUpPageObject(PageObject);

                    UIComponentAccessChainScopeCache.Release();

                    if (quitDriver)
                        _driver?.Dispose();
                });

            ExecutionStopwatch.Stop();

            string testUnitKindName = GetTestUnitKindName();
            Log.InfoWithExecutionTimeInBrackets($"Finished {testUnitKindName}", ExecutionStopwatch.Elapsed);
            Log.InfoWithExecutionTime($"Pure {testUnitKindName} execution time:", PureExecutionStopwatch.Elapsed);

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

        internal void InitDriver()
        {
            if (DriverFactory is null)
                throw new InvalidOperationException(
                    $"Failed to create an instance of {typeof(IWebDriver).FullName} as driver factory is not specified.");

            _driver = DriverFactory.Create()
                ?? throw new InvalidOperationException(
                    $"Failed to create an instance of {typeof(IWebDriver).FullName} as driver factory returned null as a driver.");

            _driver.Manage().Timeouts().SetRetryTimeout(ElementFindTimeout, ElementFindRetryInterval);

            EventBus.Publish(new DriverInitEvent(_driver));
        }

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
        /// Takes a screenshot of current page with the specified title optionally.
        /// </summary>
        /// <param name="title">The title of a screenshot.</param>
        public void TakeScreenshot(string title = null) =>
            ScreenshotTaker?.TakeScreenshot(title);

        /// <summary>
        /// Takes a snapshot (HTML or MHTML file) of current page with the specified title optionally.
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() =>
            CleanUp();
    }
}
