using System;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Atata
{
    public class SafariAtataContextBuilder : AtataContextBuilder
    {
        private Action<SafariOptions> chromeOptionsInitializer;

        internal SafariAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext = buildingContext;
            BuildingContext.DriverCreator = CreateDriver;
        }

        public RemoteWebDriver CreateDriver()
        {
            var options = new SafariOptions();
            chromeOptionsInitializer?.Invoke(options);

            return new SafariDriver(options);
        }

        public SafariAtataContextBuilder WithOptions(Action<SafariOptions> chromeOptionsInitializer)
        {
            this.chromeOptionsInitializer = chromeOptionsInitializer;
            return this;
        }
    }
}
