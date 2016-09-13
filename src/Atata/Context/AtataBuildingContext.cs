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

        public string TestName { get; set; }

        public string BaseUrl { get; set; }

        public Func<RemoteWebDriver> DriverCreator { get; set; }
    }
}
