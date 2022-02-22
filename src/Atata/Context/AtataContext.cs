using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
        }

        /// <summary>
        /// Gets or sets the current context.
        /// </summary>
        public static AtataContext Current
        {
            get
            {
                return ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic
                    ? s_currentThreadStaticContext
                    : ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal
                    ? s_currentAsyncLocalContext.Value
                    : s_currentStaticContext;
            }

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
        /// The default Artifacts directory path is <c>"{basedir}/artifacts/{build-start:yyyy-MM-dd HH_mm_ss}{test-suite-name-sanitized:/*}{test-name-sanitized:/*}"</c>.
        /// </summary>
        public DirectorySubject Artifacts { get; internal set; }

        /// <summary>
        /// Gets the <see cref="AtataNavigator"/> instance,
        /// which provides the navigation functionality between pages and windows.
        /// </summary>
        public AtataNavigator Go { get; }

        /// <summary>
        /// Gets the current page object.
        /// </summary>
        public UIComponent PageObject { get; internal set; }

        internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; } = new List<UIComponent>();

        internal bool IsNavigated { get; set; }

        internal Stopwatch ExecutionStopwatch { get; } = Stopwatch.StartNew();

        internal Stopwatch PureExecutionStopwatch { get; } = new Stopwatch();

        public ReadOnlyCollection<UIComponent> TemporarilyPreservedPageObjects
        {
            get { return TemporarilyPreservedPageObjectList.ToReadOnly(); }
        }

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
        /// Creates <see cref="AtataContextBuilder"/> instance for <see cref="AtataContext"/> configuration.
        /// Sets the value to <see cref="AtataContextBuilder.BuildingContext"/> copied from <see cref="GlobalConfiguration"/>.
        /// </summary>
        /// <returns>The created <see cref="AtataContextBuilder"/> instance.</returns>
        public static AtataContextBuilder Configure()
        {
            AtataBuildingContext buildingContext = GlobalConfiguration.BuildingContext.Clone();
            return new AtataContextBuilder(buildingContext);
        }

        internal void InitDateTimeVariables()
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

        private string GetTestUnitKindName()
        {
            return TestName != null
                ? "test"
                : TestSuiteType != null
                ? "test suite"
                : "test unit";
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
        public void RestartDriver()
        {
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
        }

        internal void CleanUpTemporarilyPreservedPageObjectList()
        {
            UIComponentResolver.CleanUpPageObjects(TemporarilyPreservedPageObjects);
            TemporarilyPreservedPageObjectList.Clear();
        }

        /// <summary>
        /// Fills the template string with variables of this <see cref="AtataContext"/> instance.
        /// The <paramref name="template"/> can contain variables wrapped with curly braces, e.g. <c>"{varName}"</c>.
        /// Variables support standard .NET formatting (<c>"{numberVar:D5}"</c> or <c>"{dateTimeVar:yyyy-MM-dd}"</c>)
        /// and extended formatting for strings
        /// (for example, <c>"{stringVar:/*}"</c> appends <c>"/"</c> to the beginning of the string, if variable is not null).
        /// <para>
        /// The list of predefined variables:
        /// <list type="bullet">
        /// <item><c>{build-start}</c></item>
        /// <item><c>{basedir}</c></item>
        /// <item><c>{artifacts}</c></item>
        /// <item><c>{test-name-sanitized}</c></item>
        /// <item><c>{test-name}</c></item>
        /// <item><c>{test-suite-name-sanitized}</c></item>
        /// <item><c>{test-suite-name}</c></item>
        /// <item><c>{test-start}</c></item>
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
        public string FillTemplateString(string template, IDictionary<string, object> additionalVariables)
        {
            template.CheckNotNull(nameof(template));

            if (!template.Contains('{'))
                return template;

            var variables = CreateVariablesDictionary();

            if (additionalVariables != null)
                foreach (var variable in additionalVariables)
                    variables[variable.Key] = variable.Value;

            // TODO: Remove the following block in Atata v2.
            template = template.
                Replace("{build-start}", $"{{build-start:{DefaultAtataContextArtifactsDirectory.DefaultDateTimeFormat}}}").
                Replace("{test-start}", $"{{test-start:{DefaultAtataContextArtifactsDirectory.DefaultDateTimeFormat}}}");

            return TemplateStringTransformer.Transform(template, variables);
        }

        private IDictionary<string, object> CreateVariablesDictionary() =>
            new Dictionary<string, object>
            {
                ["build-start"] = BuildStartInTimeZone,
                ["build-start-utc"] = BuildStartUtc,

                ["basedir"] = AppDomain.CurrentDomain.BaseDirectory,
                ["artifacts"] = Artifacts?.FullName.Value,

                ["test-name-sanitized"] = TestNameSanitized,
                ["test-name"] = TestName,
                ["test-suite-name-sanitized"] = TestSuiteNameSanitized,
                ["test-suite-name"] = TestSuiteName,
                ["test-start"] = StartedAt,
                ["test-start-utc"] = StartedAtUtc,

                ["driver-alias"] = DriverAlias
            };

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            CleanUp();
        }
    }
}
