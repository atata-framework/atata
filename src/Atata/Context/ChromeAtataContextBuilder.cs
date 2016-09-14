using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class ChromeAtataContextBuilder : AtataContextBuilder
    {
        private readonly List<Action<ChromeOptions>> optionsInitializers = new List<Action<ChromeOptions>>();

        private Func<ChromeOptions> optionsCreator;
        private Func<ChromeDriverService> driverServiceCreator;
        private string driverPath;
        private string driverExecutableFileName;
        private TimeSpan? commandTimeout;

        internal ChromeAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext = buildingContext;
            BuildingContext.DriverCreator = CreateDriver;
        }

        public RemoteWebDriver CreateDriver()
        {
            var options = optionsCreator?.Invoke() ?? new ChromeOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            ChromeDriverService driverService = driverServiceCreator?.Invoke()
                ?? (driverPath != null && driverExecutableFileName != null
                    ? ChromeDriverService.CreateDefaultService(driverPath, driverExecutableFileName)
                    : driverPath != null
                        ? ChromeDriverService.CreateDefaultService(driverPath)
                        : ChromeDriverService.CreateDefaultService());

            return new ChromeDriver(driverService, options, commandTimeout ?? TimeSpan.FromSeconds(60));
        }

        public ChromeAtataContextBuilder WithOptions(Func<ChromeOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        public ChromeAtataContextBuilder WithOptions(Action<ChromeOptions> optionsInitializer)
        {
            if (optionsInitializer != null)
                optionsInitializers.Add(optionsInitializer);
            return this;
        }

        public ChromeAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        public ChromeAtataContextBuilder WithDriverService(Func<ChromeDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        public ChromeAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        public ChromeAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        public ChromeAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
