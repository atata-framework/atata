using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public abstract class DriverAtataContextBuilder<TBuilder, TService, TOptions> : AtataContextBuilder
        where TBuilder : DriverAtataContextBuilder<TBuilder, TService, TOptions>
        where TService : DriverService
        where TOptions : DriverOptions, new()
    {
        private readonly List<Action<TService>> serviceInitializers = new List<Action<TService>>();
        private readonly List<Action<TOptions>> optionsInitializers = new List<Action<TOptions>>();

        private Func<TService> serviceFactory;
        private Func<TOptions> optionsFactory;

        private string driverPath;
        private string driverExecutableFileName;

        private TimeSpan? commandTimeout;

        protected DriverAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            BuildingContext.DriverCreator = CreateDriver;
        }

        private RemoteWebDriver CreateDriver()
        {
            var options = optionsFactory?.Invoke() ?? new TOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            TService service = serviceFactory?.Invoke() ?? CreateDefaultService();

            foreach (var serviceInitializer in serviceInitializers)
                serviceInitializer(service);

            return CreateDriver(service, options, commandTimeout ?? TimeSpan.FromSeconds(60));
        }

        private TService CreateDefaultService()
        {
            return driverPath != null && driverExecutableFileName != null
                ? CreateService(driverPath, driverExecutableFileName)
                : driverPath != null
                    ? CreateService(driverPath)
                    : CreateService();
        }

        protected abstract TService CreateService();

        protected abstract TService CreateService(string driverPath);

        protected abstract TService CreateService(string driverPath, string driverExecutableFileName);

        /// <summary>
        /// Creates the driver instance.
        /// </summary>
        /// <param name="service">The driver service.</param>
        /// <param name="options">The driver options.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>The driver instance.</returns>
        protected abstract RemoteWebDriver CreateDriver(TService service, TOptions options, TimeSpan commandTimeout);

        /// <summary>
        /// Specifies the driver options factory method.
        /// </summary>
        /// <param name="optionsFactory">The factory method of the driver options.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithOptions(Func<TOptions> optionsFactory)
        {
            optionsFactory.CheckNotNull(nameof(optionsFactory));

            this.optionsFactory = optionsFactory;
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies the driver options initialization method.
        /// </summary>
        /// <param name="optionsInitializer">The initialization method of the driver options.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithOptions(Action<TOptions> optionsInitializer)
        {
            optionsInitializer.CheckNotNull(nameof(optionsInitializer));

            optionsInitializers.Add(optionsInitializer);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            capabilityName.CheckNotNullOrWhitespace(nameof(capabilityName));

            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        /// <summary>
        /// Specifies the driver service factory method.
        /// </summary>
        /// <param name="serviceFactory">The factory method of the driver service.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithDriverService(Func<TService> serviceFactory)
        {
            serviceFactory.CheckNotNull(nameof(serviceFactory));

            this.serviceFactory = serviceFactory;
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies the driver service initialization method.
        /// </summary>
        /// <param name="serviceInitializer">The initialization method of the driver service.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithDriverService(Action<TService> serviceInitializer)
        {
            serviceInitializer.CheckNotNull(nameof(serviceInitializer));

            serviceInitializers.Add(serviceInitializer);
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies the directory containing the driver executable file.
        /// </summary>
        /// <param name="driverPath">The directory containing the driver executable file.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithDriverPath(string driverPath)
        {
            this.driverPath = driverPath.CheckNotNullOrWhitespace(nameof(driverPath));
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies the name of the driver executable file.
        /// </summary>
        /// <param name="driverExecutableFileName">The name of the driver executable file.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithDriverExecutableFileName(string driverExecutableFileName)
        {
            this.driverExecutableFileName = driverExecutableFileName.CheckNotNullOrWhitespace(nameof(driverExecutableFileName));
            return (TBuilder)this;
        }

        /// <summary>
        /// Specifies the command timeout (the maximum amount of time to wait for each command).
        /// </summary>
        /// <param name="commandTimeout">The maximum amount of time to wait for each command.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return (TBuilder)this;
        }
    }
}
