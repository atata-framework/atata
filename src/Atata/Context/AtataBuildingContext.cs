using System;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class AtataBuildingContext
    {
        internal AtataBuildingContext()
        {
        }

        public List<LogConsumerInfo> LogConsumers { get; private set; } = new List<LogConsumerInfo>();

        public List<IScreenshotConsumer> ScreenshotConsumers { get; private set; } = new List<IScreenshotConsumer>();

        /// <summary>
        /// Gets or sets the name of the test.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        public string BaseUrl { get; set; }

        public Func<RemoteWebDriver> DriverCreator { get; set; }

        public List<Action> CleanUpActions { get; private set; } = new List<Action>();

        /// <summary>
        /// Gets the retry timeout. The default value is 10 seconds.
        /// </summary>
        public TimeSpan RetryTimeout { get; internal set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Gets the retry interval. The default value is 500 milliseconds.
        /// </summary>
        public TimeSpan RetryInterval { get; internal set; } = TimeSpan.FromSeconds(0.5);

        /// <summary>
        /// Gets or sets the type of the assertion exception. The default value is typeof(Atata.AssertionException).
        /// </summary>
        public Type AssertionExceptionType { get; set; } = typeof(AssertionException);
    }
}
