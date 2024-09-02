using OpenQA.Selenium.Edge;

namespace Atata;

public class EdgeAtataContextBuilder : ChromiumAtataContextBuilder<EdgeAtataContextBuilder, EdgeDriverService, EdgeOptions>
{
    public EdgeAtataContextBuilder()
        : base(DriverAliases.Edge, "Edge")
    {
    }

    protected override EdgeDriverService CreateService() =>
        EdgeDriverService.CreateDefaultService();

    protected override EdgeDriverService CreateService(string driverPath) =>
        EdgeDriverService.CreateDefaultService(driverPath);

    protected override EdgeDriverService CreateService(string driverPath, string driverExecutableFileName) =>
        EdgeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

    protected override IWebDriver CreateDriver(EdgeDriverService service, EdgeOptions options, TimeSpan commandTimeout)
    {
        var driver = new EdgeDriver(service, options, commandTimeout);
        ReplaceLocalhostInDebuggerAddress(driver.Capabilities, "ms:edgeOptions");
        return driver;
    }

    /// <summary>
    /// Adds the additional Edge browser option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public EdgeAtataContextBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
    {
        optionName.CheckNotNullOrWhitespace(nameof(optionName));

        return WithOptions(options => options.AddAdditionalEdgeOption(optionName, optionValue));
    }
}
