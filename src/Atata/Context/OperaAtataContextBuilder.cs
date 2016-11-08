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

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsCreator">The options creator.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder WithOptions(Func<OperaOptions> optionsCreator)
        {
            this.optionsCreator = optionsCreator;
            return this;
        }

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="optionsInitializer">The options initializer.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder WithOptions(Action<OperaOptions> optionsInitializer)
        {
            optionsInitializers.Add(optionsInitializer);
            return this;
        }

        /// <summary>
        /// Adds additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        /// <summary>
        /// Specifies the creator function of the <see cref="OperaDriverService"/>.
        /// </summary>
        /// <param name="driverServiceCreator">The driver service creator function.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder WithDriverService(Func<OperaDriverService> driverServiceCreator)
        {
            this.driverServiceCreator = driverServiceCreator;
            return this;
        }

        /// <summary>
        /// Specifies the directory containing the driver executable file.
        /// </summary>
        /// <param name="driverPath">The driver path.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return this;
        }

        /// <summary>
        /// Specifies the name of the driver executable file.
        /// </summary>
        /// <param name="driverExecutableFileName">The driver executable file name.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return this;
        }

        /// <summary>
        /// Specifies the command timeout (the maximum amount of time to wait for each command).
        /// </summary>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>The <see cref="OperaAtataContextBuilder"/> instance.</returns>
        public OperaAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
