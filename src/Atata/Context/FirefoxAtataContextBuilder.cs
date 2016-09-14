using System;
using System.Collections.Generic;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class FirefoxAtataContextBuilder : AtataContextBuilder
    {
        private readonly List<Action<FirefoxOptions>> optionsInitializers = new List<Action<FirefoxOptions>>();

        private Func<FirefoxOptions> optionsCreator;
        private Func<FirefoxDriverService> driverServiceCreator;
        private string driverPath;
        private string driverExecutableFileName;
        private TimeSpan? commandTimeout;

        internal FirefoxAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext = buildingContext;
            BuildingContext.DriverCreator = CreateDriver;
        }

        public RemoteWebDriver CreateDriver()
        {
            var options = optionsCreator?.Invoke() ?? new FirefoxOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            FirefoxDriverService driverService = driverServiceCreator?.Invoke()
                ?? (driverPath != null && driverExecutableFileName != null
                    ? FirefoxDriverService.CreateDefaultService(driverPath, driverExecutableFileName)
                    : driverPath != null
                        ? FirefoxDriverService.CreateDefaultService(driverPath)
                        : FirefoxDriverService.CreateDefaultService());

            return new FirefoxDriver(driverService, options, commandTimeout ?? TimeSpan.FromSeconds(60));
        }

        public FirefoxAtataContextBuilder WithOptions(Func<FirefoxOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        public FirefoxAtataContextBuilder WithOptions(Action<FirefoxOptions> optionsInitializer)
        {
            if (optionsInitializer != null)
                optionsInitializers.Add(optionsInitializer);
            return this;
        }

        public FirefoxAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        public FirefoxAtataContextBuilder WithDriverService(Func<FirefoxDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        public FirefoxAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        public FirefoxAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        public FirefoxAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
