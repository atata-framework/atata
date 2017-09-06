using System;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Atata
{
    public class SafariAtataContextBuilder : DriverAtataContextBuilder<SafariAtataContextBuilder, SafariDriverService, SafariOptions>
    {
        public SafariAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        protected override SafariDriverService CreateService()
            => SafariDriverService.CreateDefaultService();

        protected override SafariDriverService CreateService(string driverPath)
            => SafariDriverService.CreateDefaultService(driverPath);

        protected override SafariDriverService CreateService(string driverPath, string driverExecutableFileName)
            => SafariDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override RemoteWebDriver CreateDriver(SafariDriverService service, SafariOptions options, TimeSpan commandTimeout)
            => new SafariDriver(service, options, commandTimeout);
    }
}
