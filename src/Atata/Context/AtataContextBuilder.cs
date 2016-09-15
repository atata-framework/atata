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

        public AtataContextBuilder UseDriver(Func<RemoteWebDriver> driverCreator)
        {
            driverCreator.CheckNotNull(nameof(driverCreator));

            BuildingContext.DriverCreator = driverCreator;
            return this;
        }

        public AtataContextBuilder<TLogConsumer> UseLogConsumer<TLogConsumer>(TLogConsumer consumer)
            where TLogConsumer : ILogConsumer
        {
            consumer.CheckNotNull(nameof(consumer));

            BuildingContext.LogConsumers.Add(new LogConsumerInfo(consumer));
            return new AtataContextBuilder<TLogConsumer>(consumer, BuildingContext);
        }

        public AtataContextBuilder<TLogConsumer> UseScreenshotConsumer<TLogConsumer>(TLogConsumer consumer)
            where TLogConsumer : IScreenshotConsumer
        {
            consumer.CheckNotNull(nameof(consumer));

            BuildingContext.ScreenshotConsumers.Add(consumer);
            return new AtataContextBuilder<TLogConsumer>(consumer, BuildingContext);
        }

        public AtataContextBuilder UseTestName(string name)
        {
            BuildingContext.TestName = name;
            return this;
        }

        public AtataContextBuilder UseBaseUrl(string baseUrl)
        {
            if (baseUrl != null && !Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
                throw new ArgumentException($"Invalid URL format \"{baseUrl}\".", nameof(baseUrl));

            BuildingContext.BaseUrl = baseUrl;
            return this;
        }

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
                Log = logManager
            };

            AtataContext.Current = context;

            context.LogTestStart();

            context.Log.Start("Init WebDriver");
            context.Driver = BuildingContext.DriverCreator?.Invoke() ?? new FirefoxDriver();
            context.Log.EndSection();

            context.CleanExecutionStartDateTime = DateTime.Now;
        }
    }
}
