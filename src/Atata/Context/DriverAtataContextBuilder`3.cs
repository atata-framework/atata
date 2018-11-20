using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public abstract class DriverAtataContextBuilder<TBuilder, TService, TOptions> : DriverAtataContextBuilder<TBuilder>
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

        protected DriverAtataContextBuilder(AtataBuildingContext buildingContext, string alias)
            : base(buildingContext, alias)
        {
        }

        protected sealed override RemoteWebDriver CreateDriver()
        {
            var options = optionsFactory?.Invoke() ?? new TOptions();

            foreach (var optionsInitializer in optionsInitializers)
                optionsInitializer(options);

            TService service = serviceFactory?.Invoke() ?? CreateDefaultService();

            foreach (var serviceInitializer in serviceInitializers)
                serviceInitializer(service);

            return CreateDriver(service, options, commandTimeout ?? RemoteDriverAtataContextBuilder.DefaultCommandTimeout);
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
        /// Specifies the driver options.
        /// </summary>
        /// <param name="options">The driver options.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithOptions(TOptions options)
        {
            options.CheckNotNull(nameof(options));

            optionsFactory = () => options;
            return (TBuilder)this;
        }

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
        /// Specifies the properties map for the driver options.
        /// </summary>
        /// <param name="optionsPropertiesMap">The properties map.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithOptions(Dictionary<string, object> optionsPropertiesMap)
        {
            optionsPropertiesMap.CheckNotNull(nameof(optionsPropertiesMap));

            return WithOptions(opt => AtataMapper.Map(optionsPropertiesMap, opt));
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
        /// Specifies the properties map for the driver service.
        /// </summary>
        /// <param name="servicePropertiesMap">The properties map.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithDriverService(Dictionary<string, object> servicePropertiesMap)
        {
            servicePropertiesMap.CheckNotNull(nameof(servicePropertiesMap));

            return WithDriverService(srv => AtataMapper.Map(servicePropertiesMap, srv));
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
        /// Specifies that local/current directory should be used as the directory containing the driver executable file.
        /// Uses <c>AppDomain.CurrentDomain.BaseDirectory</c> as driver folder path.
        /// This configuration option makes sense for .NET Core 2.0+ project that uses driver as a project package (hosted in the same build directory).
        /// </summary>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithLocalDriverPath()
        {
            return WithDriverPath(AppDomain.CurrentDomain.BaseDirectory);
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
        /// Specifies the host name of the service.
        /// The default value is <c>"localhost"</c>.
        /// This configuration option makes sense for .NET Core 2.0 to be set to <c>"127.0.0.1"</c> for IPv4 and <c>"[::1]"</c> for IPv6.
        /// There is a bug (https://github.com/dotnet/corefx/issues/24104) in .NET Core 2.0: each WebDriver request takes extra <c>1</c> second.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithHostName(string hostName)
        {
            return WithDriverService(x => x.HostName = hostName);
        }

        /// <summary>
        /// Specifies that the fix of driver's HTTP command execution delay should be applied.
        /// Invokes <c>WithHostName("127.0.0.1")</c> method.
        /// This configuration option makes sense for .NET Core 2.0 that works within IPv4.
        /// There is a bug (https://github.com/dotnet/corefx/issues/24104) in .NET Core 2.0: each WebDriver request takes extra <c>1</c> second.
        /// </summary>
        /// <returns>The same builder instance.</returns>
        public TBuilder WithFixOfCommandExecutionDelay()
        {
            return WithHostName("127.0.0.1");
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
