using System;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class InternetExplorerAtataContextBuilder : DriverAtataContextBuilder<InternetExplorerAtataContextBuilder, InternetExplorerDriverService, InternetExplorerOptions>
    {
        public InternetExplorerAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.InternetExplorer)
        {
        }

        protected override InternetExplorerDriverService CreateService()
            => InternetExplorerDriverService.CreateDefaultService();

        protected override InternetExplorerDriverService CreateService(string driverPath)
            => InternetExplorerDriverService.CreateDefaultService(driverPath);

        protected override InternetExplorerDriverService CreateService(string driverPath, string driverExecutableFileName)
            => InternetExplorerDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override RemoteWebDriver CreateDriver(InternetExplorerDriverService service, InternetExplorerOptions options, TimeSpan commandTimeout)
            => new InternetExplorerDriver(service, options, commandTimeout);

        /// <summary>
        /// Adds global additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add.</param>
        /// <returns>The same builder instance.</returns>
        public InternetExplorerAtataContextBuilder WithGlobalCapability(string capabilityName, object capabilityValue)
        {
            capabilityName.CheckNotNullOrWhitespace(nameof(capabilityName));

            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue, true));
        }
    }
}
