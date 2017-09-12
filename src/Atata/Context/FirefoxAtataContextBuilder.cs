using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class FirefoxAtataContextBuilder : DriverAtataContextBuilder<FirefoxAtataContextBuilder, FirefoxDriverService, FirefoxOptions>
    {
        public FirefoxAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.Firefox)
        {
        }

        protected override FirefoxDriverService CreateService()
            => FirefoxDriverService.CreateDefaultService();

        protected override FirefoxDriverService CreateService(string driverPath)
            => FirefoxDriverService.CreateDefaultService(driverPath);

        protected override FirefoxDriverService CreateService(string driverPath, string driverExecutableFileName)
            => FirefoxDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override RemoteWebDriver CreateDriver(FirefoxDriverService service, FirefoxOptions options, TimeSpan commandTimeout)
            => new FirefoxDriver(service, options, commandTimeout);

        /// <summary>
        /// Adds arguments to be used in launching the Firefox browser.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The same builder instance.</returns>
        public FirefoxAtataContextBuilder WithArguments(params string[] arguments)
        {
            return WithOptions(options => options.AddArguments(arguments));
        }
    }
}
