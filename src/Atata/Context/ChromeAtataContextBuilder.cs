using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class ChromeAtataContextBuilder : DriverAtataContextBuilder<ChromeAtataContextBuilder, ChromeDriverService, ChromeOptions>
    {
        public ChromeAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.Chrome, "Chrome")
        {
        }

        protected override ChromeDriverService CreateService()
            => ChromeDriverService.CreateDefaultService();

        protected override ChromeDriverService CreateService(string driverPath)
            => ChromeDriverService.CreateDefaultService(driverPath);

        protected override ChromeDriverService CreateService(string driverPath, string driverExecutableFileName)
            => ChromeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override RemoteWebDriver CreateDriver(ChromeDriverService service, ChromeOptions options, TimeSpan commandTimeout)
            => new ChromeDriver(service, options, commandTimeout);

        /// <summary>
        /// Adds arguments to be appended to the Chrome.exe command line.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The same builder instance.</returns>
        public ChromeAtataContextBuilder WithArguments(params string[] arguments)
        {
            return WithArguments(arguments.AsEnumerable());
        }

        /// <summary>
        /// Adds arguments to be appended to the Chrome.exe command line.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The same builder instance.</returns>
        public ChromeAtataContextBuilder WithArguments(IEnumerable<string> arguments)
        {
            return WithOptions(options => options.AddArguments(arguments));
        }

        /// <summary>
        /// Adds global additional capability to the driver options.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add.</param>
        /// <returns>The same builder instance.</returns>
        public ChromeAtataContextBuilder WithGlobalCapability(string capabilityName, object capabilityValue)
        {
            capabilityName.CheckNotNullOrWhitespace(nameof(capabilityName));

            return WithOptions(options => options.AddAdditionalCapability(capabilityName, capabilityValue, true));
        }
    }
}
