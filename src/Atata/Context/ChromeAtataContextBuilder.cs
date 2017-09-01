using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class ChromeAtataContextBuilder : DriverAtataContextBuilder<ChromeAtataContextBuilder, ChromeDriverService, ChromeOptions>
    {
        internal ChromeAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
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
    }
}
