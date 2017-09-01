using System;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class OperaAtataContextBuilder : DriverAtataContextBuilder<OperaAtataContextBuilder, OperaDriverService, OperaOptions>
    {
        internal OperaAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
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
    }
}
