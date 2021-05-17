using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Remote;

namespace Atata
{
    /// <summary>
    /// Represents the Atata context, the entry point for the test set-up.
    /// </summary>
    public sealed class AtataContext : IDisposable
    {
        private static readonly object LockObject = new object();

#if NET46 || NETSTANDARD2_0
        private static readonly System.Threading.AsyncLocal<AtataContext> CurrentAsyncLocalContext = new System.Threading.AsyncLocal<AtataContext>();

#endif
        private static AtataContextModeOfCurrent modeOfCurrent = AtataContextModeOfCurrent.ThreadStatic;

        [ThreadStatic]
        private static AtataContext currentThreadStaticContext;

        private static AtataContext currentStaticContext;

        private string testName;

        private string testFixtureName;

        private bool disposed;

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
                    ? currentThreadStaticContext
#if NET46 || NETSTANDARD2_0
                    : ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal
                    ? CurrentAsyncLocalContext.Value
#endif
                    : currentStaticContext;
            }

            set
            {
                if (ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
                    currentThreadStaticContext = value;
#if NET46 || NETSTANDARD2_0
                else if (ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
                    CurrentAsyncLocalContext.Value = value;
#endif
                else
                    currentStaticContext = value;
            }
        }

        /// <summary>
        /// Gets or sets the mode of <see cref="Current"/> property.
        /// The default value is <see cref="AtataContextModeOfCurrent.ThreadStatic"/>.
        /// </summary>
        public static AtataContextModeOfCurrent ModeOfCurrent
        {
            get => modeOfCurrent;
            set
            {
                modeOfCurrent = value;

                RetrySettings.ThreadBoundary = value == AtataContextModeOfCurrent.ThreadStatic
                    ? RetrySettingsThreadBoundary.ThreadStatic
#if NET46 || NETSTANDARD2_0
                    : value == AtataContextModeOfCurrent.AsyncLocal
                    ? RetrySettingsThreadBoundary.AsyncLocal
#endif
                    : RetrySettingsThreadBoundary.Static;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Current"/> property use thread-static approach (value unique for each thread).
        /// The default value is <see langword="true"/>.
        /// </summary>
        [Obsolete("Use ModeOfCurrent instead.")] // Obsolete since v1.5.0.
        public static bool IsThreadStatic
        {
            get => ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic;
            set => ModeOfCurrent = value ? AtataContextModeOfCurrent.ThreadStatic : AtataContextModeOfCurrent.Static;
        }

        /// <summary>
        /// Gets the global configuration.
        /// </summary>
        public static AtataContextBuilder GlobalConfiguration { get; } = new AtataContextBuilder(new AtataBuildingContext());

        /// <summary>
        /// Gets the build start date and time.
        /// Contains the same value for all the tests being executed within one build.
        /// </summary>
        public static DateTime? BuildStart { get; private set; }

        internal IDriverFactory DriverFactory { get; set; }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        public RemoteWebDriver Driver { get; internal set; }

        /// <summary>
        /// Gets the driver alias.
        /// </summary>
        public string DriverAlias { get; internal set; }

        /// <summary>
        /// Gets the instance of the log manager.
        /// </summary>
        public ILogManager Log { get; internal set; }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public string TestName
        {
            get => testName;
            internal set
            {
                testName = value;
                TestNameSanitized = value.SanitizeForFileName();
            }
        }

        /// <summary>
        /// Gets the name of the test sanitized for file path/name.
        /// </summary>
        public string TestNameSanitized { get; private set; }

        /// <summary>
        /// Gets the name of the test fixture/class.
        /// </summary>
        public string TestFixtureName
        {
            get => testFixtureName;
            internal set
            {
                testFixtureName = value;
                TestFixtureNameSanitized = value.SanitizeForFileName();
            }
        }

        /// <summary>
        /// Gets the name of the test fixture sanitized for file path/name.
        /// </summary>
        public string TestFixtureNameSanitized { get; private set; }

        /// <summary>
        /// Gets the test fixture type.
        /// </summary>
        public Type TestFixtureType { get; internal set; }

        /// <summary>
        /// Gets the test start date and time.
        /// </summary>
        [Obsolete("Use " + nameof(StartedAt) + " instead.")] // Obsolete since v1.9.0.
        public DateTime TestStart => StartedAt;

        /// <summary>
        /// Gets the local date/time of the start.
        /// </summary>
        public DateTime StartedAt { get; private set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets the base retry timeout.
        /// The default value is <c>5</c> seconds.
        /// </summary>
        [Obsolete("Use BaseRetryTimeout instead.")] // Obsolete since v0.17.0.
        public TimeSpan RetryTimeout => BaseRetryTimeout;

        /// <summary>
        /// Gets the base retry interval.
        /// The default value is <c>500</c> milliseconds.
        /// </summary>
        [Obsolete("Use BaseRetryInterval instead.")] // Obsolete since v0.17.0.
        public TimeSpan RetryInterval => BaseRetryInterval;

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

        internal List<Action<RemoteWebDriver>> OnDriverCreatedActions { get; set; }

        /// <summary>
        /// Gets the list of actions to perform during <see cref="AtataContext"/> cleanup.
        /// </summary>
        public List<Action> CleanUpActions { get; internal set; }

        /// <summary>
        /// Gets the context of the attributes.
        /// </summary>
        public AtataAttributesContext Attributes { get; internal set; }

        /// <summary>
        /// Gets the <see cref="DirectorySubject"/> of Artifacts directory.
        /// Artifacts directory can contain any files produced during test execution, logs, screenshots, downloads, etc.
        /// The default Artifacts directory path is <c>"{basedir}/artifacts/{build-start:yyyy-MM-dd HH_mm_ss}{test-fixture-name-sanitized:/*}{test-name-sanitized:/*}"</c>.
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

        internal UIComponentScopeCache UIComponentScopeCache { get; } = new UIComponentScopeCache();

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
        /// Creates <see cref="AtataContextBuilder"/> instance for <see cref="AtataContext"/> configuration.
        /// Sets the value to <see cref="AtataContextBuilder.BuildingContext"/> copied from <see cref="GlobalConfiguration"/>.
        /// </summary>
        /// <returns>The created <see cref="AtataContextBuilder"/> instance.</returns>
        public static AtataContextBuilder Configure()
        {
            AtataBuildingContext buildingContext = GlobalConfiguration.BuildingContext.Clone();
            return new AtataContextBuilder(buildingContext);
        }

        internal static void InitGlobalVariables()
        {
            if (BuildStart == null)
            {
                lock (LockObject)
                {
                    BuildStart = BuildStart ?? DateTime.Now;
                }
            }
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

            Current.Log.Info(logMessageBuilder.ToString());
        }

        private IEnumerable<string> GetTestFullNameParts()
        {
            if (TestFixtureType != null)
                yield return TestFixtureType.Namespace;

            if (TestFixtureName != null)
                yield return TestFixtureName;

            if (TestName != null)
                yield return TestName;
        }

        private string GetTestUnitKindName()
        {
            return TestName != null
                ? "test"
                : TestFixtureType != null
                ? "test fixture"
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
            if (disposed)
                return;

            PureExecutionStopwatch.Stop();

            ExecuteCleanUpActions();

            Log.ExecuteSection(
                new LogSection("Clean up AtataContext"),
                () =>
                {
                    CleanUpTemporarilyPreservedPageObjectList();

                    if (PageObject != null)
                        UIComponentResolver.CleanUpPageObject(PageObject);

                    UIComponentScopeCache.Clear();

                    if (quitDriver)
                        Driver?.Dispose();
                });

            ExecutionStopwatch.Stop();

            string testUnitKindName = GetTestUnitKindName();
            Log.InfoWithExecutionTimeInBrackets($"Finished {testUnitKindName}", ExecutionStopwatch.Elapsed);
            Log.InfoWithExecutionTime($"Pure {testUnitKindName} execution time:", PureExecutionStopwatch.Elapsed);

            Log = null;

            if (Current == this)
                Current = null;

            disposed = true;

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
            Driver = DriverFactory.Create()
                ?? throw new InvalidOperationException($"Failed to create an instance of {nameof(RemoteWebDriver)} as driver factory returned 'null' as a driver.");

            Driver.Manage().Timeouts().SetRetryTimeout(ElementFindTimeout, ElementFindRetryInterval);

            if (OnDriverCreatedActions != null)
                foreach (Action<RemoteWebDriver> action in OnDriverCreatedActions)
                    action(Driver);
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

                    Driver.Dispose();

                    InitDriver();
                });
        }

        internal void CleanUpTemporarilyPreservedPageObjectList()
        {
            UIComponentResolver.CleanUpPageObjects(TemporarilyPreservedPageObjects);
            TemporarilyPreservedPageObjectList.Clear();
        }

        private void ExecuteCleanUpActions()
        {
            foreach (Action action in CleanUpActions)
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Log.Error("Clean up action failure.", e);
                }
            }
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
        /// <item><c>{test-fixture-name-sanitized}</c></item>
        /// <item><c>{test-fixture-name}</c></item>
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
                ["build-start"] = BuildStart,

                ["basedir"] = AppDomain.CurrentDomain.BaseDirectory,
                ["artifacts"] = Artifacts?.FullName.Value,

                ["test-name-sanitized"] = TestNameSanitized,
                ["test-name"] = TestName,
                ["test-fixture-name-sanitized"] = TestFixtureNameSanitized,
                ["test-fixture-name"] = TestFixtureName,
                ["test-start"] = StartedAt,

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
