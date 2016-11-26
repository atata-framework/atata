using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

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
        /// Sets the name of the test.
        /// </summary>
        /// <param name="name">The name of the test.</param>
        /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
        public AtataContextBuilder UseTestName(string name)
        {
            BuildingContext.TestName = name;
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
        /// Adds the action to perform on <see cref="AtataContext"/> clean up.
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
                TestName = BuildingContext.TestName,
                BaseUrl = BuildingContext.BaseUrl,
                Log = logManager,
                CleanUpActions = BuildingContext.CleanUpActions,
                RetryTimeout = BuildingContext.RetryTimeout,
                RetryInterval = BuildingContext.RetryInterval
            };

            AtataContext.Current = context;

            context.LogTestStart();

            context.Log.Start("Init WebDriver");

            context.Driver = BuildingContext.DriverCreator?.Invoke() ?? new FirefoxDriver();
            context.Driver.Manage().Timeouts().SetRetryTimeout(BuildingContext.RetryTimeout, BuildingContext.RetryInterval);

            context.Log.EndSection();

            context.CleanExecutionStartDateTime = DateTime.Now;
        }
    }
}
