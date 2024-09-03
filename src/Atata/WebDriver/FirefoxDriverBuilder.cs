using OpenQA.Selenium.Firefox;

namespace Atata;

public class FirefoxDriverBuilder : WebDriverBuilder<FirefoxDriverBuilder, FirefoxDriverService, FirefoxOptions>
{
    public FirefoxDriverBuilder()
        : base(WebDriverAliases.Firefox, "Firefox")
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
    public FirefoxDriverBuilder WithArguments(params string[] arguments) =>
        WithArguments(arguments.AsEnumerable());

    /// <summary>
    /// Adds arguments to be used in launching the Firefox browser.
    /// </summary>
    /// <param name="arguments">The arguments.</param>
    /// <returns>The same builder instance.</returns>
    public FirefoxDriverBuilder WithArguments(IEnumerable<string> arguments) =>
        WithOptions(options => options.AddArguments(arguments));

    /// <summary>
    /// Adds the additional Firefox browser option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public FirefoxDriverBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
    {
        optionName.CheckNotNullOrWhitespace(nameof(optionName));

        return WithOptions(options => options.AddAdditionalFirefoxOption(optionName, optionValue));
    }
}
