using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class FirefoxAtataContextBuilder : DriverAtataContextBuilder<FirefoxAtataContextBuilder, FirefoxDriverService, FirefoxOptions>
    {
        internal FirefoxAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
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
    }
}
