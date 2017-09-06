using System;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class EdgeAtataContextBuilder : DriverAtataContextBuilder<EdgeAtataContextBuilder, EdgeDriverService, EdgeOptions>
    {
        public EdgeAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        protected override EdgeDriverService CreateService()
            => EdgeDriverService.CreateDefaultService();

        protected override EdgeDriverService CreateService(string driverPath)
            => EdgeDriverService.CreateDefaultService(driverPath);

        protected override EdgeDriverService CreateService(string driverPath, string driverExecutableFileName)
            => EdgeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override RemoteWebDriver CreateDriver(EdgeDriverService service, EdgeOptions options, TimeSpan commandTimeout)
            => new EdgeDriver(service, options, commandTimeout);
    }
}
