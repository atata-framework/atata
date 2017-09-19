using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Atata
{
    /// <summary>
    /// Represents the builder of <see cref="AtataContext"/>.
    /// </summary>
    public class AtataContextBuilder
    {
        public AtataContextBuilder(AtataBuildingContext buildingContext)
        {
            BuildingContext = buildingContext.CheckNotNull(nameof(buildingContext));
        }

        /// <summary>
        /// Gets the building context.
        /// </summary>
        public AtataBuildingContext BuildingContext { get; internal set; }

        /// <summary>
        /// Use the driver factory.
        /// </summary>
        /// <typeparam name="TDriverFactory">The type of the driver factory.</typeparam>
        /// <param name="driverFactory">The driver factory.</param>
        /// <returns>The <typeparamref name="TDriverFactory"/> instance.</returns>
        public TDriverFactory UseDriver<TDriverFactory>(TDriverFactory driverFactory)
            where TDriverFactory : AtataContextBuilder, IDriverFactory
        {
            driverFactory.CheckNotNull(nameof(driverFactory));

            BuildingContext.DriverFactories.Add(driverFactory);
            BuildingContext.DriverFactoryToUse = driverFactory;

            return driverFactory;
        }

        /// <summary>
        /// Sets the alias of the driver to use.
        /// </summary>
        /// <param name="alias">The alias of the driver.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseDriver(string alias)
        {
            alias.CheckNotNullOrWhitespace(nameof(alias));

            IDriverFactory driverFactory = BuildingContext.DriverFactories.LastOrDefault(x => alias.Equals(x.Alias, StringComparison.CurrentCultureIgnoreCase));

            if (driverFactory != null)
                BuildingContext.DriverFactoryToUse = driverFactory;
            else if (UsePredefinedDriver(alias) == null)
                throw new ArgumentException($"No driver with \"{alias}\" alias defined.", nameof(alias));

            return this;
        }

        private IDriverFactory UsePredefinedDriver(string alias)
        {
            switch (alias.ToLowerInvariant())
            {
                case DriverAliases.Chrome:
                    return UseChrome();
                case DriverAliases.Firefox:
                    return UseFirefox();
                case DriverAliases.InternetExplorer:
                    return UseInternetExplorer();
                case DriverAliases.Safari:
                    return UseSafari();
                case DriverAliases.Opera:
                    return UseOpera();
                case DriverAliases.Edge:
                    return UseEdge();
                case DriverAliases.PhantomJS:
                    return UsePhantomJS();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Use custom driver factory method.
        /// </summary>
        /// <param name="driverFactory">The driver factory method.</param>
        /// <returns>The <see cref="CustomDriverAtataContextBuilder"/> instance.</returns>
        public CustomDriverAtataContextBuilder UseDriver(Func<RemoteWebDriver> driverFactory)
        {
            driverFactory.CheckNotNull(nameof(driverFactory));

            return UseDriver(new CustomDriverAtataContextBuilder(BuildingContext, driverFactory));
        }

        /// <summary>
        /// Use the <see cref="ChromeDriver"/>.
        /// </summary>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder UseChrome()
        {
            return UseDriver(new ChromeAtataContextBuilder(BuildingContext));
        }

        /// <summary>
        /// Use the <see cref="FirefoxDriver"/>.
        /// </summary>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder UseFirefox()
        {
            return UseDriver(new FirefoxAtataContextBuilder(BuildingContext));
        }

        /// <summary>
        /// Use the <see cref="InternetExplorerDriver"/>.
        /// </summary>
        /// <returns>The <see cref="InternetExplorerAtataContextBuilder"/> instance.</returns>
        public InternetExplorerAtataContextBuilder UseInternetExplorer()
        {
            return UseDriver(new InternetExplorerAtataContextBuilder(BuildingContext));
        }

        /// <summary>
        /// Use the <see cref="EdgeDriver"/>.
        /// </summary>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder UseEdge()
        {
            return UseDriver(new EdgeAtataContextBuilder(BuildingContext));
        }

        /// <summary>
        /// Use the <see cref="OperaDriver"/>.
        /// </summary>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder UseOpera()
        {
            return UseDriver(new OperaAtataContextBuilder(BuildingContext));
        }

        /// <summary>
        /// Use the <see cref="PhantomJSDriver"/>.
        /// </summary>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder UsePhantomJS()
        {
            return UseDriver(new PhantomJSAtataContextBuilder(BuildingContext));
        }

        /// <summary>
        /// Use the <see cref="SafariDriver"/>.
        /// </summary>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder UseSafari()
        {
            return UseDriver(new SafariAtataContextBuilder(BuildingContext));
        }

        /// <summary>
        /// Use the <see cref="RemoteWebDriver"/>.
        /// </summary>
        /// <returns>The <see cref="RemoteDriverAtataContextBuilder"/> instance.</returns>
        public RemoteDriverAtataContextBuilder UseRemoteDriver()
        {
            return UseDriver(new RemoteDriverAtataContextBuilder(BuildingContext));
        }

        /// <summary>
        /// Adds the log consumer.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="consumer">The log consumer.</param>
        /// <returns>The <see cref="AtataContextBuilder{TLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<TLogConsumer> AddLogConsumer<TLogConsumer>(TLogConsumer consumer)
            where TLogConsumer : ILogConsumer
        {
            consumer.CheckNotNull(nameof(consumer));

            BuildingContext.LogConsumers.Add(new LogConsumerInfo(consumer));
            return new AtataContextBuilder<TLogConsumer>(consumer, BuildingContext);
        }

        /// <summary>
        /// Adds the log consumer.
        /// </summary>
        /// <param name="typeNameOrAlias">The type name or alias of the log consumer.</param>
        /// <returns>The <see cref="AtataContextBuilder{TLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<ILogConsumer> AddLogConsumer(string typeNameOrAlias)
        {
            ILogConsumer consumer = LogConsumerAliases.Resolve(typeNameOrAlias);
            return AddLogConsumer(consumer);
        }

        /// <summary>
        /// Adds the <see cref="TraceLogConsumer"/> instance that uses <see cref="Trace"/> class for logging.
        /// </summary>
        /// <returns>The <see cref="AtataContextBuilder{TraceLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<TraceLogConsumer> AddTraceLogging()
        {
            return AddLogConsumer(new TraceLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="DebugLogConsumer"/> instance that uses <see cref="Debug"/> class for logging.
        /// </summary>
        /// <returns>The <see cref="AtataContextBuilder{DebugLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<DebugLogConsumer> AddDebugLogging()
        {
            return AddLogConsumer(new DebugLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="NUnitTestContextLogConsumer"/> instance that uses NUnit.Framework.TestContext class for logging.
        /// </summary>
        /// <returns>The <see cref="AtataContextBuilder{NUnitTestContextLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<NUnitTestContextLogConsumer> AddNUnitTestContextLogging()
        {
            return AddLogConsumer(new NUnitTestContextLogConsumer());
        }

        /// <summary>
        /// Adds the <see cref="NLogConsumer"/> instance that uses NLog.Logger class for logging.
        /// </summary>
        /// <param name="loggerName">The name of the logger.</param>
        /// <returns>The <see cref="AtataContextBuilder{NLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<NLogConsumer> AddNLogLogging(string loggerName = null)
        {
            return AddLogConsumer(new NLogConsumer(loggerName));
        }

        /// <summary>
        /// Adds the screenshot consumer.
        /// </summary>
        /// <typeparam name="TScreenshotConsumer">The type of the screenshot consumer.</typeparam>
        /// <param name="consumer">The screenshot consumer.</param>
        /// <returns>The <see cref="AtataContextBuilder{TLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<TScreenshotConsumer> AddScreenshotConsumer<TScreenshotConsumer>(TScreenshotConsumer consumer)
            where TScreenshotConsumer : IScreenshotConsumer
        {
            consumer.CheckNotNull(nameof(consumer));

            BuildingContext.ScreenshotConsumers.Add(consumer);
            return new AtataContextBuilder<TScreenshotConsumer>(consumer, BuildingContext);
        }

        /// <summary>
        /// Adds the screenshot consumer.
        /// </summary>
        /// <param name="typeNameOrAlias">The type name or alias of the log consumer.</param>
        /// <returns>The <see cref="AtataContextBuilder{TLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<IScreenshotConsumer> AddScreenshotConsumer(string typeNameOrAlias)
        {
            IScreenshotConsumer consumer = ScreenshotConsumerAliases.Resolve(typeNameOrAlias);

            return AddScreenshotConsumer(consumer);
        }

        /// <summary>
        /// Adds the <see cref="FileScreenshotConsumer"/> instance for the screenshot saving to file.
        /// </summary>
        /// <returns>The <see cref="AtataContextBuilder{FileScreenshotConsumer}"/> instance.</returns>
        public AtataContextBuilder<FileScreenshotConsumer> AddScreenshotFileSaving()
        {
            return AddScreenshotConsumer(new FileScreenshotConsumer());
        }

        /// <summary>
        /// Sets the factory method of the test name.
        /// </summary>
        /// <param name="testNameFactory">The factory method of the test name.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseTestName(Func<string> testNameFactory)
        {
            testNameFactory.CheckNotNull(nameof(testNameFactory));

            BuildingContext.TestNameFactory = testNameFactory;
            return this;
        }

        /// <summary>
        /// Sets the name of the test.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseTestName(string testName)
        {
            testName.CheckNotNull(nameof(testName));

            BuildingContext.TestNameFactory = () => testName;
            return this;
        }

        /// <summary>
        /// Sets the base URL.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseBaseUrl(string baseUrl)
        {
            if (baseUrl != null && !Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
                throw new ArgumentException($"Invalid URL format \"{baseUrl}\".", nameof(baseUrl));

            BuildingContext.BaseUrl = baseUrl;
            return this;
        }

        /// <summary>
        /// Sets the retry timeout for a search of element/control. The default value is 10 seconds.
        /// </summary>
        /// <param name="timeout">The retry timeout.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseRetryTimeout(TimeSpan timeout)
        {
            BuildingContext.RetryTimeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the retry interval for a search of element/control. The default value is 500 milliseconds.
        /// </summary>
        /// <param name="interval">The retry interval.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseRetryInterval(TimeSpan interval)
        {
            BuildingContext.RetryInterval = interval;
            return this;
        }

        /// <summary>
        /// Sets the type of the assertion exception. The default value is typeof(Atata.AssertionException).
        /// </summary>
        /// <param name="exceptionType">The type of the exception.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseAssertionExceptionType(Type exceptionType)
        {
            exceptionType.Check(typeof(Exception).IsAssignableFrom, nameof(exceptionType), $"The type should be inherited from {nameof(Exception)}.");

            BuildingContext.AssertionExceptionType = exceptionType;
            return this;
        }

        /// <summary>
        /// Sets the type of the assertion exception. The default value is typeof(Atata.AssertionException).
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseAssertionExceptionType<TException>()
            where TException : Exception
        {
            return UseAssertionExceptionType(typeof(TException));
        }

        /// <summary>
        /// Adds the action to perform during <see cref="AtataContext"/> cleanup.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder OnCleanUp(Action action)
        {
            if (action != null)
                BuildingContext.CleanUpActions.Add(action);
            return this;
        }

        /// <summary>
        /// Defines that the name of the test should be taken from the NUnit test.
        /// </summary>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseNUnitTestName()
        {
            return UseTestName(ResolveNUnitTestName);
        }

        private static string ResolveNUnitTestName()
        {
            dynamic testContext = GetNUnitTestContext();
            return testContext.Test.Name;
        }

        /// <summary>
        /// Defines that an error occurred during the NUnit test execution should be added to the log during the cleanup.
        /// </summary>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder LogNUnitError()
        {
            return OnCleanUp(() =>
            {
                dynamic testResult = GetNUnitTestResult();

                if (IsNUnitTestResultFailed(testResult))
                    AtataContext.Current.Log.Error((string)testResult.Message, (string)testResult.StackTrace);
            });
        }

        /// <summary>
        /// Defines that an error occurred during the NUnit test execution should be captured by a screenshot during the cleanup.
        /// </summary>
        /// <param name="title">The screenshot title.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder TakeScreenshotOnNUnitError(string title = "Failed")
        {
            return OnCleanUp(() =>
            {
                dynamic testResult = GetNUnitTestResult();

                if (IsNUnitTestResultFailed(testResult))
                    AtataContext.Current.Log.Screenshot(title);
            });
        }

        private static dynamic GetNUnitTestContext()
        {
            Type testContextType = Type.GetType("NUnit.Framework.TestContext,nunit.framework", true);
            PropertyInfo currentContextProperty = testContextType.GetPropertyWithThrowOnError("CurrentContext");

            return currentContextProperty.GetStaticValue();
        }

        private static dynamic GetNUnitTestResult()
        {
            dynamic testContext = GetNUnitTestContext();
            return testContext.Result;
        }

        private static bool IsNUnitTestResultFailed(dynamic testResult)
        {
            return testResult.Outcome.Status.ToString().Contains("Fail");
        }

        /// <summary>
        /// Clears the <see cref="BuildingContext"/>.
        /// </summary>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder Clear()
        {
            BuildingContext = new AtataBuildingContext();
            return this;
        }

        /// <summary>
        /// Sets up the context.
        /// </summary>
        [Obsolete("Use Build() instead.")] // Obsolete since v0.14.0.
        public void SetUp()
        {
            Build();
        }

        /// <summary>
        /// Builds the <see cref="AtataContext" /> instance and sets it to <see cref="AtataContext.Current" /> property.
        /// </summary>
        /// <returns>The created <see cref="AtataContext"/> instance.</returns>
        public AtataContext Build()
        {
            AtataContext.InitGlobalVariables();

            LogManager logManager = new LogManager();

            foreach (var logConsumerItem in BuildingContext.LogConsumers)
                logManager.Use(logConsumerItem.Consumer, logConsumerItem.MinLevel, logConsumerItem.LogSectionFinish);

            foreach (var screenshotConsumer in BuildingContext.ScreenshotConsumers)
                logManager.Use(screenshotConsumer);

            AtataContext context = new AtataContext
            {
                TestName = BuildingContext.TestNameFactory?.Invoke(),
                BaseUrl = BuildingContext.BaseUrl,
                Log = logManager,
                CleanUpActions = BuildingContext.CleanUpActions,
                RetryTimeout = BuildingContext.RetryTimeout,
                RetryInterval = BuildingContext.RetryInterval,
                AssertionExceptionType = BuildingContext.AssertionExceptionType
            };

            AtataContext.Current = context;

            context.LogTestStart();

            context.Log.Start("Set up AtataContext", LogLevel.Trace);

            if (context.BaseUrl != null)
                context.Log.Trace($"Set: BaseUrl={context.BaseUrl}");

            context.Log.Trace($"Set: RetryTimeout={context.RetryTimeout.ToIntervalString()}; RetryInterval={context.RetryInterval.ToIntervalString()}");

            context.Driver = BuildingContext.DriverFactoryToUse?.Create() ?? new FirefoxDriver();
            context.DriverAlias = BuildingContext.DriverFactoryToUse?.Alias ?? DriverAliases.Firefox;

            context.Log.Trace($"Set: Driver={context.Driver.GetType().Name}{BuildingContext.DriverFactoryToUse?.Alias?.ToFormattedString(" (alias={0})")}");

            context.Driver.Manage().Timeouts().SetRetryTimeout(BuildingContext.RetryTimeout, BuildingContext.RetryInterval);

            context.Log.EndSection();

            context.CleanExecutionStartDateTime = DateTime.Now;

            return context;
        }
    }
}
