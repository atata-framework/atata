using System;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public class CustomDriverAtataContextBuilder : AtataContextBuilder, IDriverFactory
    {
        private readonly Func<RemoteWebDriver> driverFactory;

        public CustomDriverAtataContextBuilder(AtataBuildingContext buildingContext, Func<RemoteWebDriver> driverFactory)
            : base(buildingContext)
        {
            this.driverFactory = driverFactory.CheckNotNull(nameof(driverFactory));
        }

        public string Alias { get; private set; }

        RemoteWebDriver IDriverFactory.Create()
        {
            return driverFactory();
        }

        /// <summary>
        /// Specifies the driver alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>The same builder instance.</returns>
        public CustomDriverAtataContextBuilder WithAlias(string alias)
        {
            alias.CheckNotNullOrWhitespace(nameof(alias));

            Alias = alias;
            return this;
        }
    }
}
