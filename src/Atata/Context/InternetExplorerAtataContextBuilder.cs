using System;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace Atata
{
    public class InternetExplorerAtataContextBuilder : DriverAtataContextBuilder<InternetExplorerAtataContextBuilder, InternetExplorerDriverService, InternetExplorerOptions>
    {
        public InternetExplorerAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.InternetExplorer, "Internet Explorer")
        {
        }

        protected override InternetExplorerDriverService CreateService()
            => InternetExplorerDriverService.CreateDefaultService();

        protected override InternetExplorerDriverService CreateService(string driverPath)
            => InternetExplorerDriverService.CreateDefaultService(driverPath);

        protected override InternetExplorerDriverService CreateService(string driverPath, string driverExecutableFileName)
            => InternetExplorerDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override IWebDriver CreateDriver(InternetExplorerDriverService service, InternetExplorerOptions options, TimeSpan commandTimeout)
            => new InternetExplorerDriver(service, options, commandTimeout);

        /// <summary>
        /// Adds the additional Internet Explorer browser option to the driver options.
        /// </summary>
        /// <param name="optionName">The name of the option to add.</param>
        /// <param name="optionValue">The value of the option to add.</param>
        /// <returns>The same builder instance.</returns>
        public InternetExplorerAtataContextBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
        {
            optionName.CheckNotNullOrWhitespace(nameof(optionName));

            return WithOptions(options => options.AddAdditionalInternetExplorerOption(optionName, optionValue));
        }
    }
}
