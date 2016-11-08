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

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsCreator">The options creator.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder WithOptions(Func<FirefoxOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsInitializer">The options initializer.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder WithOptions(Action<FirefoxOptions> optionsInitializer)
        {
            optionsInitializers.Add(optionsInitializer);
            return this;
        }

        /// <summary>
        /// Adds additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        /// <summary>
        /// Specifies the creator function of the <see cref="FirefoxDriverService"/>.
        /// </summary>
        /// <param name="driverServiceCreator">The driver service creator function.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder WithDriverService(Func<FirefoxDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        /// <summary>
        /// Specifies the directory containing the driver executable file.
        /// </summary>
        /// <param name="driverPath">The driver path.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        /// <summary>
        /// Specifies the name of the driver executable file.
        /// </summary>
        /// <param name="driverExecutableFileName">The driver executable file name.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        /// <summary>
        /// Specifies the command timeout (the maximum amount of time to wait for each command).
        /// </summary>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
        public FirefoxAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
