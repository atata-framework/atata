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
    }
}
