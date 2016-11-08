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

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsCreator">The options creator.</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder WithOptions(Func<EdgeOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsInitializer">The options initializer.</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder WithOptions(Action<EdgeOptions> optionsInitializer)
        {
            optionsInitializers.Add(optionsInitializer);
            return this;
        }

        /// <summary>
        /// Adds additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        /// <summary>
        /// Specifies the creator function of the <see cref="EdgeDriverService"/>.
        /// </summary>
        /// <param name="driverServiceCreator">The driver service creator function.</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder WithDriverService(Func<EdgeDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        /// <summary>
        /// Specifies the directory containing the driver executable file.
        /// </summary>
        /// <param name="driverPath">The driver path.</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        /// <summary>
        /// Specifies the name of the driver executable file.
        /// </summary>
        /// <param name="driverExecutableFileName">The driver executable file name.</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        /// <summary>
        /// Specifies the command timeout (the maximum amount of time to wait for each command).
        /// </summary>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
        public EdgeAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
