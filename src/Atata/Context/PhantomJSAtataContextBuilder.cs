using System;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class PhantomJSAtataContextBuilder : DriverAtataContextBuilder<PhantomJSAtataContextBuilder, PhantomJSDriverService, PhantomJSOptions>
    {
        public PhantomJSAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        protected override PhantomJSDriverService CreateService()
            => PhantomJSDriverService.CreateDefaultService();

        protected override PhantomJSDriverService CreateService(string driverPath)
            => PhantomJSDriverService.CreateDefaultService(driverPath);

        protected override PhantomJSDriverService CreateService(string driverPath, string driverExecutableFileName)
            => PhantomJSDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override RemoteWebDriver CreateDriver(PhantomJSDriverService service, PhantomJSOptions options, TimeSpan commandTimeout)
            => new PhantomJSDriver(service, options, commandTimeout);
    }
}
