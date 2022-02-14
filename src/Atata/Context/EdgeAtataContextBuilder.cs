using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Atata
{
    public class EdgeAtataContextBuilder : DriverAtataContextBuilder<EdgeAtataContextBuilder, EdgeDriverService, EdgeOptions>
    {
        public EdgeAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.Edge, "Edge")
        {
        }

        protected override EdgeDriverService CreateService()
            => EdgeDriverService.CreateDefaultService();

        protected override EdgeDriverService CreateService(string driverPath)
            => EdgeDriverService.CreateDefaultService(driverPath);

        protected override EdgeDriverService CreateService(string driverPath, string driverExecutableFileName)
            => EdgeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override IWebDriver CreateDriver(EdgeDriverService service, EdgeOptions options, TimeSpan commandTimeout)
            => new EdgeDriver(service, options, commandTimeout);
    }
}
