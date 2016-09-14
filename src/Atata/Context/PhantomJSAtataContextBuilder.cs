using System;
using System.Collections.Generic;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class PhantomJSAtataContextBuilder : AtataContextBuilder
    {
        private readonly List<Action<PhantomJSOptions>> optionsInitializers = new List<Action<PhantomJSOptions>>();

        private Func<PhantomJSOptions> optionsCreator;
        private Func<PhantomJSDriverService> driverServiceCreator;
        private string driverPath;
        private string driverExecutableFileName;
        private TimeSpan? commandTimeout;

        internal PhantomJSAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext = buildingContext;
            BuildingContext.DriverCreator = CreateDriver;
        }

        public RemoteWebDriver CreateDriver()
        {
            var options = optionsCreator?.Invoke() ?? new PhantomJSOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            PhantomJSDriverService driverService = driverServiceCreator?.Invoke()
                ?? (driverPath != null && driverExecutableFileName != null
                    ? PhantomJSDriverService.CreateDefaultService(driverPath, driverExecutableFileName)
                    : driverPath != null
                        ? PhantomJSDriverService.CreateDefaultService(driverPath)
                        : PhantomJSDriverService.CreateDefaultService());

            return new PhantomJSDriver(driverService, options, commandTimeout ?? TimeSpan.FromSeconds(60));
        }

        public PhantomJSAtataContextBuilder WithOptions(Func<PhantomJSOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        public PhantomJSAtataContextBuilder WithOptions(Action<PhantomJSOptions> optionsInitializer)
        {
            if (optionsInitializer != null)
                optionsInitializers.Add(optionsInitializer);
            return this;
        }

        public PhantomJSAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        public PhantomJSAtataContextBuilder WithDriverService(Func<PhantomJSDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        public PhantomJSAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        public PhantomJSAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        public PhantomJSAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
