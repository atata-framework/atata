using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Atata
{
    public class FirefoxAtataContextBuilder : DriverAtataContextBuilder<FirefoxAtataContextBuilder, FirefoxDriverService, FirefoxOptions>
    {
        public FirefoxAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.Firefox, "Firefox")
        {
        }

        protected override FirefoxDriverService CreateService()
            => FirefoxDriverService.CreateDefaultService();

        protected override FirefoxDriverService CreateService(string driverPath)
            => FirefoxDriverService.CreateDefaultService(driverPath);

        protected override FirefoxDriverService CreateService(string driverPath, string driverExecutableFileName)
            => FirefoxDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override IWebDriver CreateDriver(FirefoxDriverService service, FirefoxOptions options, TimeSpan commandTimeout)
            => new FirefoxDriver(service, options, commandTimeout);

        /// <summary>
        /// Adds arguments to be used in launching the Firefox browser.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The same builder instance.</returns>
        public FirefoxAtataContextBuilder WithArguments(params string[] arguments) =>
            WithArguments(arguments.AsEnumerable());

        /// <summary>
        /// Adds arguments to be used in launching the Firefox browser.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The same builder instance.</returns>
        public FirefoxAtataContextBuilder WithArguments(IEnumerable<string> arguments) =>
            WithOptions(options => options.AddArguments(arguments));

        /// <summary>
        /// Adds the additional Firefox browser option to the driver options.
        /// </summary>
        /// <param name="optionName">The name of the option to add.</param>
        /// <param name="optionValue">The value of the option to add.</param>
        /// <returns>The same builder instance.</returns>
        public FirefoxAtataContextBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
        {
            optionName.CheckNotNullOrWhitespace(nameof(optionName));

            return WithOptions(options => options.AddAdditionalFirefoxOption(optionName, optionValue));
        }
    }
}
