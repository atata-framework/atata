using System;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class CustomDriverAtataContextBuilder : DriverAtataContextBuilder<CustomDriverAtataContextBuilder>
    {
        private readonly Func<RemoteWebDriver> driverFactory;

        public CustomDriverAtataContextBuilder(AtataBuildingContext buildingContext, Func<RemoteWebDriver> driverFactory)
            : base(buildingContext)
        {
            this.driverFactory = driverFactory.CheckNotNull(nameof(driverFactory));
        }

        protected override RemoteWebDriver CreateDriver()
        {
            return driverFactory();
        }
    }
}
