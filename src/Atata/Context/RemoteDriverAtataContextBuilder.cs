using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class RemoteDriverAtataContextBuilder : DriverAtataContextBuilder<RemoteDriverAtataContextBuilder>
    {
        public const string DefaultRemoteServerUrl = "http://127.0.0.1:4444/wd/hub";

        /// <summary>
        /// The default command timeout is <c>60</c> seconds.
        /// </summary>
        public static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromSeconds(60);

        private readonly List<Action<DriverOptions>> optionsInitializers = new List<Action<DriverOptions>>();

        private Uri remoteAddress;

        private Func<DriverOptions> optionsFactory;

        private Func<ICapabilities> capabilitiesFactory;

        private TimeSpan? commandTimeout;

        public RemoteDriverAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.Remote)
        {
        }

        protected sealed override RemoteWebDriver CreateDriver()
        {
            ICapabilities capabilities = CreateCapabilities();

            return CreateDriver(remoteAddress, capabilities, commandTimeout ?? DefaultCommandTimeout);
        }

        protected virtual RemoteWebDriver CreateDriver(Uri remoteAddress, ICapabilities capabilities, TimeSpan commandTimeout)
        {
            return new RemoteWebDriver(remoteAddress, capabilities, commandTimeout);
        }

        protected virtual ICapabilities CreateCapabilities()
        {
            var options = optionsFactory?.Invoke();

            if (options != null)
            {
                foreach (var optionsInitializer in optionsInitializers)
                    optionsInitializer(options);

                return options.ToCapabilities();
            }
            else
            {
                return capabilitiesFactory?.Invoke()
                    ?? throw new InvalidOperationException($"Type or instance of {nameof(DriverOptions)} is not set. Use one of {nameof(RemoteDriverAtataContextBuilder)}.{nameof(WithOptions)} methods to set driver options type or instance.");
            }
        }

        /// <summary>
        /// Specifies the remote address URI.
        /// </summary>
        /// <param name="remoteAddress">URI containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithRemoteAddress(Uri remoteAddress)
        {
            this.remoteAddress = remoteAddress;
            return this;
        }

        /// <summary>
        /// Specifies the remote address URI.
        /// </summary>
        /// <param name="remoteAddress">URI string containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4444/wd/hub).</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithRemoteAddress(string remoteAddress)
        {
            remoteAddress.CheckNotNullOrWhitespace(nameof(remoteAddress));

            this.remoteAddress = new Uri(remoteAddress);
            return this;
        }

        /// <summary>
        /// Specifies the type of the driver options.
        /// </summary>
        /// <typeparam name="TOptions">The type of the options.</typeparam>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithOptions<TOptions>()
            where TOptions : DriverOptions, new()
        {
            optionsFactory = () => new TOptions();
            return this;
        }

        /// <summary>
        /// Specifies the driver options.
        /// </summary>
        /// <param name="options">The driver options.</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithOptions(DriverOptions options)
        {
            options.CheckNotNull(nameof(options));

            optionsFactory = () => options;
            return this;
        }

        /// <summary>
        /// Specifies the driver options factory method.
        /// </summary>
        /// <param name="optionsFactory">The factory method of the driver options.</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithOptions(Func<DriverOptions> optionsFactory)
        {
            optionsFactory.CheckNotNull(nameof(optionsFactory));

            this.optionsFactory = optionsFactory;
            return this;
        }

        /// <summary>
        /// Specifies the driver options initialization method.
        /// </summary>
        /// <param name="optionsInitializer">The initialization method of the driver options.</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithOptions(Action<DriverOptions> optionsInitializer)
        {
            optionsInitializer.CheckNotNull(nameof(optionsInitializer));

            optionsInitializers.Add(optionsInitializer);
            return this;
        }

        /// <summary>
        /// Specifies the capabilities.
        /// </summary>
        /// <param name="capabilities">The driver capabilities.</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithCapabilities(ICapabilities capabilities)
        {
            capabilities.CheckNotNull(nameof(capabilities));

            capabilitiesFactory = () => capabilities;
            return this;
        }

        /// <summary>
        /// Specifies the capabilities factory method.
        /// </summary>
        /// <param name="capabilitiesFactory">The factory method of the driver capabilities.</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithCapabilities(Func<ICapabilities> capabilitiesFactory)
        {
            capabilitiesFactory.CheckNotNull(nameof(capabilitiesFactory));

            this.capabilitiesFactory = capabilitiesFactory;
            return this;
        }

        /// <summary>
        /// Adds the capability.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithCapability(string capabilityName, object capabilityValue)
        {
            capabilityName.CheckNotNullOrWhitespace(nameof(capabilityName));

            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue));
        }

        /// <summary>
        /// Specifies the command timeout (the maximum amount of time to wait for each command).
        /// </summary>
        /// <param name="commandTimeout">The maximum amount of time to wait for each command.</param>
        /// <returns>The same builder instance.</returns>
        public RemoteDriverAtataContextBuilder WithCommandTimeout(TimeSpan commandTimeout)
        {
            this.commandTimeout = commandTimeout;
            return this;
        }
    }
}
