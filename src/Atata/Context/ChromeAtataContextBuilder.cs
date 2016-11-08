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

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsCreator">The options creator.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder WithOptions(Func<ChromeOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsInitializer">The options initializer.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder WithOptions(Action<ChromeOptions> optionsInitializer)
        {
            optionsInitializers.Add(optionsInitializer);
            return this;
        }

        /// <summary>
        /// Adds additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        /// <summary>
        /// Specifies the creator function of the <see cref="ChromeDriverService"/>.
        /// </summary>
        /// <param name="driverServiceCreator">The driver service creator function.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder WithDriverService(Func<ChromeDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        /// <summary>
        /// Specifies the directory containing the driver executable file.
        /// </summary>
        /// <param name="driverPath">The driver path.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        /// <summary>
        /// Specifies the name of the driver executable file.
        /// </summary>
        /// <param name="driverExecutableFileName">The driver executable file name.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        /// <summary>
        /// Specifies the command timeout (the maximum amount of time to wait for each command).
        /// </summary>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
        public ChromeAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
