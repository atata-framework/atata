#nullable enable

using OpenQA.Selenium.Chrome;

namespace Atata;

public class ChromeDriverBuilder : ChromiumDriverBuilder<ChromeDriverBuilder, ChromeDriverService, ChromeOptions>
{
    public ChromeDriverBuilder()
        : base(WebDriverAliases.Chrome, "Chrome")
    {
    }

    protected override ChromeDriverService CreateService() =>
        ChromeDriverService.CreateDefaultService();

    protected override ChromeDriverService CreateService(string driverPath) =>
        ChromeDriverService.CreateDefaultService(driverPath);

    protected override ChromeDriverService CreateService(string driverPath, string driverExecutableFileName) =>
        ChromeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

    protected override IWebDriver CreateDriver(ChromeDriverService service, ChromeOptions options, TimeSpan commandTimeout)
    {
        ChromeDriver driver = new(service, options, commandTimeout);
        ReplaceLocalhostInDebuggerAddress(driver.Capabilities, "goog:chromeOptions");
        return driver;
    }

    /// <summary>
    /// Adds the additional Chrome browser option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public ChromeDriverBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
    {
        optionName.CheckNotNullOrWhitespace(nameof(optionName));

        return WithOptions(options => options.AddAdditionalChromeOption(optionName, optionValue));
    }
}
