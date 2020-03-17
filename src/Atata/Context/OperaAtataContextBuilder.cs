using System;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class OperaAtataContextBuilder : DriverAtataContextBuilder<OperaAtataContextBuilder, OperaDriverService, OperaOptions>
    {
        public OperaAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.Opera)
        {
        }

        protected override OperaDriverService CreateService()
            => OperaDriverService.CreateDefaultService();

        protected override OperaDriverService CreateService(string driverPath)
            => OperaDriverService.CreateDefaultService(driverPath);

        protected override OperaDriverService CreateService(string driverPath, string driverExecutableFileName)
            => OperaDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override RemoteWebDriver CreateDriver(OperaDriverService service, OperaOptions options, TimeSpan commandTimeout)
            => new OperaDriver(service, options, commandTimeout);

        /// <summary>
        /// Adds arguments to be appended to the Opera.exe command line.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The same builder instance.</returns>
        public OperaAtataContextBuilder WithArguments(params string[] arguments)
        {
            return WithOptions(options => options.AddArguments(arguments));
        }

        /// <summary>
        /// Adds global additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add.</param>
        /// <returns>The same builder instance.</returns>
        public OperaAtataContextBuilder WithGlobalCapability(string capabilityName, object capabilityValue)
        {
            capabilityName.CheckNotNullOrWhitespace(nameof(capabilityName));

            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue, true));
        }
    }
}
