using System;
using System.Collections.Generic;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class OperaAtataContextBuilder : AtataContextBuilder
    {
        private readonly List<Action<OperaOptions>> optionsInitializers = new List<Action<OperaOptions>>();

        private Func<OperaOptions> optionsCreator;
        private Func<OperaDriverService> driverServiceCreator;
        private string driverPath;
        private string driverExecutableFileName;
        private TimeSpan? commandTimeout;

        internal OperaAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext = buildingContext;
            BuildingContext.DriverCreator = CreateDriver;
        }

        public RemoteWebDriver CreateDriver()
        {
            var options = optionsCreator?.Invoke() ?? new OperaOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            OperaDriverService driverService = driverServiceCreator?.Invoke()
                ?? (driverPath != null && driverExecutableFileName != null
                    ? OperaDriverService.CreateDefaultService(driverPath, driverExecutableFileName)
                    : driverPath != null
                        ? OperaDriverService.CreateDefaultService(driverPath)
                        : OperaDriverService.CreateDefaultService());

            return new OperaDriver(driverService, options, commandTimeout ?? TimeSpan.FromSeconds(60));
        }

        public OperaAtataContextBuilder WithOptions(Func<OperaOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        public OperaAtataContextBuilder WithOptions(Action<OperaOptions> optionsInitializer)
        {
            if (optionsInitializer != null)
                optionsInitializers.Add(optionsInitializer);
            return this;
        }

        public OperaAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        public OperaAtataContextBuilder WithDriverService(Func<OperaDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        public OperaAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        public OperaAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        public OperaAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
