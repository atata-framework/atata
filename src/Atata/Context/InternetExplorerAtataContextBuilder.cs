using System;
using System.Collections.Generic;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class InternetExplorerAtataContextBuilder : AtataContextBuilder
    {
        private readonly List<Action<InternetExplorerOptions>> optionsInitializers = new List<Action<InternetExplorerOptions>>();

        private Func<InternetExplorerOptions> optionsCreator;
        private Func<InternetExplorerDriverService> driverServiceCreator;
        private string driverPath;
        private string driverExecutableFileName;
        private TimeSpan? commandTimeout;

        internal InternetExplorerAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext = buildingContext;
            BuildingContext.DriverCreator = CreateDriver;
        }

        public RemoteWebDriver CreateDriver()
        {
            var options = optionsCreator?.Invoke() ?? new InternetExplorerOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            InternetExplorerDriverService driverService = driverServiceCreator?.Invoke()
                ?? (driverPath != null && driverExecutableFileName != null
                    ? InternetExplorerDriverService.CreateDefaultService(driverPath, driverExecutableFileName)
                    : driverPath != null
                        ? InternetExplorerDriverService.CreateDefaultService(driverPath)
                        : InternetExplorerDriverService.CreateDefaultService());

            return new InternetExplorerDriver(driverService, options, commandTimeout ?? TimeSpan.FromSeconds(60));
        }

        public InternetExplorerAtataContextBuilder WithOptions(Func<InternetExplorerOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        public InternetExplorerAtataContextBuilder WithOptions(Action<InternetExplorerOptions> optionsInitializer)
        {
            if (optionsInitializer != null)
                optionsInitializers.Add(optionsInitializer);
            return this;
        }

        public InternetExplorerAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        public InternetExplorerAtataContextBuilder WithDriverService(Func<InternetExplorerDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        public InternetExplorerAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        public InternetExplorerAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        public InternetExplorerAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
