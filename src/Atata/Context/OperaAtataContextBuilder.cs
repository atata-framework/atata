using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Opera;

namespace Atata
{
    public class OperaAtataContextBuilder : DriverAtataContextBuilder<OperaAtataContextBuilder, OperaDriverService, OperaOptions>
    {
        public OperaAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext, DriverAliases.Opera, "Opera")
        {
        }

        protected override OperaDriverService CreateService()
            => OperaDriverService.CreateDefaultService();

        protected override OperaDriverService CreateService(string driverPath)
            => OperaDriverService.CreateDefaultService(driverPath);

        protected override OperaDriverService CreateService(string driverPath, string driverExecutableFileName)
            => OperaDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

        protected override IWebDriver CreateDriver(OperaDriverService service, OperaOptions options, TimeSpan commandTimeout)
            => new OperaDriver(service, options, commandTimeout);

        /// <summary>
        /// Adds arguments to be appended to the Opera.exe command line.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The same builder instance.</returns>
        public OperaAtataContextBuilder WithArguments(params string[] arguments)
        {
            return WithArguments(arguments.AsEnumerable());
        }

        /// <summary>
        /// Adds arguments to be appended to the Opera.exe command line.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The same builder instance.</returns>
        public OperaAtataContextBuilder WithArguments(IEnumerable<string> arguments)
        {
            return WithOptions(options => options.AddArguments(arguments));
        }

        /// <summary>
        /// Adds the additional Opera browser option to the driver options.
        /// </summary>
        /// <param name="optionName">The name of the option to add.</param>
        /// <param name="optionValue">The value of the option to add.</param>
        /// <returns>The same builder instance.</returns>
        public OperaAtataContextBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
        {
            optionName.CheckNotNullOrWhitespace(nameof(optionName));

            return WithOptions(options => options.AddAdditionalOperaOption(optionName, optionValue));
        }
    }
}
