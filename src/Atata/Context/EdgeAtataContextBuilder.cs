using System;
using System.Collections.Generic;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class EdgeAtataContextBuilder : AtataContextBuilder
    {
        private readonly List<Action<EdgeOptions>> optionsInitializers = new List<Action<EdgeOptions>>();

        private Func<EdgeOptions> optionsCreator;
        private Func<EdgeDriverService> driverServiceCreator;
        private string driverPath;
        private string driverExecutableFileName;
        private TimeSpan? commandTimeout;

        internal EdgeAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext = buildingContext;
            BuildingContext.DriverCreator = CreateDriver;
        }

        public RemoteWebDriver CreateDriver()
        {
            var options = optionsCreator?.Invoke() ?? new EdgeOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            EdgeDriverService driverService = driverServiceCreator?.Invoke()
                ?? (driverPath != null && driverExecutableFileName != null
                    ? EdgeDriverService.CreateDefaultService(driverPath, driverExecutableFileName)
                    : driverPath != null
                        ? EdgeDriverService.CreateDefaultService(driverPath)
                        : EdgeDriverService.CreateDefaultService());

            return new EdgeDriver(driverService, options, commandTimeout ?? TimeSpan.FromSeconds(60));
        }

        public EdgeAtataContextBuilder WithOptions(Func<EdgeOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        public EdgeAtataContextBuilder WithOptions(Action<EdgeOptions> optionsInitializer)
        {
            if (optionsInitializer != null)
                optionsInitializers.Add(optionsInitializer);
            return this;
        }

        public EdgeAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        public EdgeAtataContextBuilder WithDriverService(Func<EdgeDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        public EdgeAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        public EdgeAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        public EdgeAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
