using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
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
        /// Adds the <see cref="ConsoleLogConsumer"/> instance that uses <see cref="Console"/> class for logging.
        /// </summary>
        /// <returns>The <see cref="AtataContextBuilder{ConsoleLogConsumer}"/> instance.</returns>
        public AtataContextBuilder<ConsoleLogConsumer> AddConsoleLogging()
        {
            return AddLogConsumer(new ConsoleLogConsumer());
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
        /// By default uses <c>"Logs\{build-start}\{test-name}"</c> as folder path format,
        /// <c>"{screenshot-number:D2} - {screenshot-pageobjectfullname}{screenshot-title: - *}"</c> as file name format
        /// and <c>Png</c> as image format.
        /// Example of screenshot file path using default settings: <c>"Logs\2018-03-03 14_34_04\SampleTest\01 - Home page - Screenshot title.png"</c>.
        /// Available path variables: <c>{build-start}</c>, <c>{test-name}</c>, <c>{test-start}</c>, <c>{driver-alias}</c>, <c>{screenshot-number}</c>, <c>{screenshot-title}</c>, <c>{screenshot-pageobjectname}</c>, <c>{screenshot-pageobjecttypename}</c>, <c>{screenshot-pageobjectfullname}</c>.
        /// Path variables support the formatting.
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
        /// Sets the base retry timeout. The default value is 5 seconds.
        /// </summary>
        /// <param name="timeout">The retry timeout.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        [Obsolete("Use UseBaseRetryTimeout instead.")] // Obsolete since v0.17.0.
        public AtataContextBuilder UseRetryTimeout(TimeSpan timeout)
        {
            return UseBaseRetryTimeout(timeout);
        }

        /// <summary>
        /// Sets the base retry interval. The default value is 500 milliseconds.
        /// </summary>
        /// <param name="interval">The retry interval.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        [Obsolete("Use UseBaseRetryInterval instead.")] // Obsolete since v0.17.0.
        public AtataContextBuilder UseRetryInterval(TimeSpan interval)
        {
            return UseBaseRetryInterval(interval);
        }

        /// <summary>
        /// Sets the base retry timeout. The default value is 5 seconds.
        /// </summary>
        /// <param name="timeout">The retry timeout.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseBaseRetryTimeout(TimeSpan timeout)
        {
            BuildingContext.BaseRetryTimeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the base retry interval. The default value is 500 milliseconds.
        /// </summary>
        /// <param name="interval">The retry interval.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseBaseRetryInterval(TimeSpan interval)
        {
            BuildingContext.BaseRetryInterval = interval;
            return this;
        }

        /// <summary>
        /// Sets the element find timeout.
        /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryTimeout"/>, which is equal to 5 seconds by default.
        /// </summary>
        /// <param name="timeout">The retry timeout.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseElementFindTimeout(TimeSpan timeout)
        {
            BuildingContext.ElementFindTimeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the element find retry interval.
        /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryInterval"/>, which is equal to 500 milliseconds by default.
        /// </summary>
        /// <param name="interval">The retry interval.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseElementFindRetryInterval(TimeSpan interval)
        {
            BuildingContext.ElementFindRetryInterval = interval;
            return this;
        }

        /// <summary>
        /// Sets the waiting timeout.
        /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryTimeout"/>, which is equal to 5 seconds by default.
        /// </summary>
        /// <param name="timeout">The retry timeout.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseWaitingTimeout(TimeSpan timeout)
        {
            BuildingContext.WaitingTimeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the waiting retry interval.
        /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryInterval"/>, which is equal to 500 milliseconds by default.
        /// </summary>
        /// <param name="interval">The retry interval.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseWaitingRetryInterval(TimeSpan interval)
        {
            BuildingContext.WaitingRetryInterval = interval;
            return this;
        }

        /// <summary>
        /// Sets the verification timeout.
        /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryTimeout"/>, which is equal to 5 seconds by default.
        /// </summary>
        /// <param name="timeout">The retry timeout.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseVerificationTimeout(TimeSpan timeout)
        {
            BuildingContext.VerificationTimeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the verification retry interval.
        /// The default value is taken from <see cref="AtataBuildingContext.BaseRetryInterval"/>, which is equal to 500 milliseconds by default.
        /// </summary>
        /// <param name="interval">The retry interval.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseVerificationRetryInterval(TimeSpan interval)
        {
            BuildingContext.VerificationRetryInterval = interval;
            return this;
        }

        /// <summary>
        /// Sets the culture. The default value is <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseCulture(CultureInfo culture)
        {
            BuildingContext.Culture = culture;
            return this;
        }

        /// <summary>
        /// Sets the culture by the name. The default value is <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        /// <param name="cultureName">The name of the culture.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseCulture(string cultureName)
        {
            return UseCulture(new CultureInfo(cultureName));
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
        /// Adds the action to perform during <see cref="AtataContext"/> building.
        /// It will be executed at the beginning of the build after the log is set up.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder OnBuilding(Action action)
        {
            if (action != null)
                BuildingContext.OnBuildingActions.Add(action);
            return this;
        }

        /// <summary>
        /// Adds the action to perform after <see cref="AtataContext"/> building.
        /// It will be executed at the end of the build after the driver is created.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder OnBuilt(Action action)
        {
            if (action != null)
                BuildingContext.OnBuiltActions.Add(action);
            return this;
        }

        /// <summary>
        /// Adds the action to perform after the driver is created.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder OnDriverCreated(Action<RemoteWebDriver> action)
        {
            if (action != null)
                BuildingContext.OnDriverCreatedActions.Add(action);
            return this;
        }

        /// <summary>
        /// Adds the action to perform after the driver is created.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder OnDriverCreated(Action action)
        {
            return action != null ? OnDriverCreated(x => action()) : this;
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
        /// Builds the <see cref="AtataContext" /> instance and sets it to <see cref="AtataContext.Current" /> property.
        /// </summary>
        /// <returns>The created <see cref="AtataContext"/> instance.</returns>
        public AtataContext Build()
        {
            AtataContext.InitGlobalVariables();

            ValidateBuildingContextBeforeBuild();

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
                OnDriverCreatedActions = BuildingContext.OnDriverCreatedActions?.ToList() ?? new List<Action<RemoteWebDriver>>(),
                CleanUpActions = BuildingContext.CleanUpActions?.ToList() ?? new List<Action>(),
                BaseRetryTimeout = BuildingContext.BaseRetryTimeout,
                BaseRetryInterval = BuildingContext.BaseRetryInterval,
                ElementFindTimeout = BuildingContext.ElementFindTimeout,
                ElementFindRetryInterval = BuildingContext.ElementFindRetryInterval,
                WaitingTimeout = BuildingContext.WaitingTimeout,
                WaitingRetryInterval = BuildingContext.WaitingRetryInterval,
                VerificationTimeout = BuildingContext.VerificationTimeout,
                VerificationRetryInterval = BuildingContext.VerificationRetryInterval,
                Culture = BuildingContext.Culture ?? CultureInfo.CurrentCulture,
                AssertionExceptionType = BuildingContext.AssertionExceptionType
            };

            AtataContext.Current = context;

            OnBuilding(context);

            if (context.BaseUrl != null)
                context.Log.Trace($"Set: BaseUrl={context.BaseUrl}");

            LogRetrySettings(context);

            if (BuildingContext.Culture != null)
            {
                if (AtataContext.IsThreadStatic)
                {
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = BuildingContext.Culture;
                }
                else
                {
#if NET40
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = BuildingContext.Culture;
#else
                    CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = BuildingContext.Culture;
#endif
                }

                context.Log.Trace($"Set: Culture={BuildingContext.Culture.Name}");
            }

            context.DriverFactory = BuildingContext.DriverFactoryToUse;
            context.DriverAlias = BuildingContext.DriverFactoryToUse.Alias;

            context.InitDriver();

            context.Log.Trace($"Set: Driver={context.Driver.GetType().Name}{BuildingContext.DriverFactoryToUse?.Alias?.ToFormattedString(" (alias={0})")}");

            OnBuilt(context);

            return context;
        }

        private void OnBuilding(AtataContext context)
        {
            context.LogTestStart();

            context.Log.Start("Set up AtataContext", LogLevel.Trace);

            if (BuildingContext.OnBuildingActions != null)
            {
                foreach (Action action in BuildingContext.OnBuildingActions)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        context.Log.Error($"On {nameof(AtataContext)} building action failure.", e);
                    }
                }
            }
        }

        private void OnBuilt(AtataContext context)
        {
            if (BuildingContext.OnBuiltActions != null)
            {
                foreach (Action action in BuildingContext.OnBuiltActions)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        context.Log.Error($"On {nameof(AtataContext)} built action failure.", e);
                    }
                }
            }

            context.Log.EndSection();

            context.CleanExecutionStartDateTime = DateTime.Now;
        }

        private void LogRetrySettings(AtataContext context)
        {
            string messageFormat = "Set: {0}Timeout={1}; {0}RetryInterval={2}";
            context.Log.Trace(messageFormat, "ElementFind", context.ElementFindTimeout.ToIntervalString(), context.ElementFindRetryInterval.ToIntervalString());
            context.Log.Trace(messageFormat, "Waiting", context.WaitingTimeout.ToIntervalString(), context.WaitingRetryInterval.ToIntervalString());
            context.Log.Trace(messageFormat, "Verification", context.VerificationTimeout.ToIntervalString(), context.VerificationRetryInterval.ToIntervalString());
        }

        private void ValidateBuildingContextBeforeBuild()
        {
            if (BuildingContext.DriverFactoryToUse == null)
                throw new InvalidOperationException($"Cannot build {nameof(AtataContext)} as no driver is specified. Use one of 'Use*' methods to specify which to driver to use, e.g.: AtataContext.Configure().UseChrome().Build();");
        }
    }
}
