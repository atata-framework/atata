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

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsCreator">The options creator.</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder WithOptions(Func<PhantomJSOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsInitializer">The options initializer.</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder WithOptions(Action<PhantomJSOptions> optionsInitializer)
        {
            optionsInitializers.Add(optionsInitializer);
            return this;
        }

        /// <summary>
        /// Adds additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        /// <summary>
        /// Specifies the creator function of the <see cref="PhantomJSDriverService"/>.
        /// </summary>
        /// <param name="driverServiceCreator">The driver service creator function.</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder WithDriverService(Func<PhantomJSDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        /// <summary>
        /// Specifies the directory containing the driver executable file.
        /// </summary>
        /// <param name="driverPath">The driver path.</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        /// <summary>
        /// Specifies the name of the driver executable file.
        /// </summary>
        /// <param name="driverExecutableFileName">The driver executable file name.</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        /// <summary>
        /// Specifies the command timeout (the maximum amount of time to wait for each command).
        /// </summary>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>The <see cref="PhantomJSAtataContextBuilder"/> instance.</returns>
        public PhantomJSAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
