using System;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Atata
{
    public class SafariAtataContextBuilder : AtataContextBuilder
    {
        private readonly List<Action<SafariOptions>> optionsInitializers = new List<Action<SafariOptions>>();

        private Func<SafariOptions> optionsCreator;
        private Func<SafariDriverService> driverServiceCreator;
        private string driverPath;
        private string driverExecutableFileName;
        private TimeSpan? commandTimeout;

        internal SafariAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext = buildingContext;
            BuildingContext.DriverCreator = CreateDriver;
        }

        public RemoteWebDriver CreateDriver()
        {
            var options = optionsCreator?.Invoke() ?? new SafariOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            SafariDriverService driverService = driverServiceCreator?.Invoke()
                ?? (driverPath != null && driverExecutableFileName != null
                    ? SafariDriverService.CreateDefaultService(driverPath, driverExecutableFileName)
                    : driverPath != null
                        ? SafariDriverService.CreateDefaultService(driverPath)
                        : SafariDriverService.CreateDefaultService());

            return new SafariDriver(driverService, options, commandTimeout ?? TimeSpan.FromSeconds(60));
        }

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsCreator">The options creator.</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder WithOptions(Func<SafariOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsInitializer">The options initializer.</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder WithOptions(Action<SafariOptions> optionsInitializer)
        {
            optionsInitializers.Add(optionsInitializer);
            return this;
        }

        /// <summary>
        /// Adds additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        /// <summary>
        /// Specifies the creator function of the <see cref="SafariDriverService"/>.
        /// </summary>
        /// <param name="driverServiceCreator">The driver service creator function.</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder WithDriverService(Func<SafariDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        /// <summary>
        /// Specifies the directory containing the driver executable file.
        /// </summary>
        /// <param name="driverPath">The driver path.</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        /// <summary>
        /// Specifies the name of the driver executable file.
        /// </summary>
        /// <param name="driverExecutableFileName">The driver executable file name.</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        /// <summary>
        /// Specifies the command timeout (the maximum amount of time to wait for each command).
        /// </summary>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
        public SafariAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
