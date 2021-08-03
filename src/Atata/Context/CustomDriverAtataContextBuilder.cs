using System;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class CustomDriverAtataContextBuilder : DriverAtataContextBuilder<CustomDriverAtataContextBuilder>
    {
        private readonly Func<RemoteWebDriver> _driverFactory;

        public CustomDriverAtataContextBuilder(AtataBuildingContext buildingContext, Func<RemoteWebDriver> driverFactory)
            : base(buildingContext)
        {
            _driverFactory = driverFactory.CheckNotNull(nameof(driverFactory));
        }

        protected override RemoteWebDriver CreateDriver() =>
            _driverFactory.Invoke();
    }
}
