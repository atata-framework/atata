using System;
using System.Diagnostics;
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
    public class AtataContextBuilder
    {
        internal AtataContextBuilder(AtataBuildingContext buildingContext)
        {
            BuildingContext = buildingContext;
        }

        public AtataBuildingContext BuildingContext { get; internal set; }

        /// <summary>
        /// Use custom driver creator function.
        /// </summary>
        /// <param name="driverCreator">The builder.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseDriver(Func<RemoteWebDriver> driverCreator)
        {
            driverCreator.CheckNotNull(nameof(driverCreator));

            BuildingContext.DriverCreator = driverCreator;
            return this;
        }

        /// <summary>
        /// Use the <see cref="ChromeDriver"/>.
        /// </summary>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder UseChrome()
        {
            return new ChromeAtataContextBuilder(BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="FirefoxDriver"/>.
        /// </summary>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder UseFirefox()
        {
            return new FirefoxAtataContextBuilder(BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="InternetExplorerDriver"/>.
        /// </summary>
        /// <returns>The <see cref="InternetExplorerAtataContextBuilder"/> instance.</returns>
        public InternetExplorerAtataContextBuilder UseInternetExplorer()
        {
            return new InternetExplorerAtataContextBuilder(BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="EdgeDriver"/>.
        /// </summary>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder UseEdge()
        {
            return new EdgeAtataContextBuilder(BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="OperaDriver"/>.
        /// </summary>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder UseOpera()
        {
            return new OperaAtataContextBuilder(BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="PhantomJSDriver"/>.
        /// </summary>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder UsePhantomJS()
        {
            return new PhantomJSAtataContextBuilder(BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="SafariDriver"/>.
        /// </summary>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder UseSafari()
        {
            return new SafariAtataContextBuilder(BuildingContext);
        }

        /// <summary>
        /// Use the <see cref="RemoteWebDriver"/>.
        /// </summary>
        /// <returns>The <see cref="RemoteDriverAtataContextBuilder"/> instance.</returns>
        public RemoteDriverAtataContextBuilder UseRemoteDriver()
        {
            return new RemoteDriverAtataContextBuilder(BuildingContext);
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
        /// Sets up the context.
        /// </summary>
        public void SetUp()
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

            context.Driver = BuildingContext.DriverCreator?.Invoke() ?? new FirefoxDriver();

            context.Log.Trace($"Set: Driver={context.Driver.GetType().Name}");

            context.Driver.Manage().Timeouts().SetRetryTimeout(BuildingContext.RetryTimeout, BuildingContext.RetryInterval);

            context.Log.EndSection();

            context.CleanExecutionStartDateTime = DateTime.Now;
        }
    }
}
