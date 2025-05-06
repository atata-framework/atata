using OpenQA.Selenium.Chrome;

namespace Atata;

/// <summary>
/// Represents a builder for creating and configuring <see cref="ChromeDriver"/> instances.
/// </summary>
public class ChromeDriverBuilder : ChromiumDriverBuilder<ChromeDriverBuilder, ChromeDriverService, ChromeOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChromeDriverBuilder"/> class.
    /// </summary>
    public ChromeDriverBuilder()
        : base(WebDriverAliases.Chrome, "Chrome")
    {
    }

    /// <inheritdoc/>
    protected override ChromeDriverService CreateService() =>
        ChromeDriverService.CreateDefaultService();

    /// <inheritdoc/>
    protected override ChromeDriverService CreateService(string driverPath) =>
        ChromeDriverService.CreateDefaultService(driverPath);

    /// <inheritdoc/>
    protected override ChromeDriverService CreateService(string driverPath, string driverExecutableFileName) =>
        ChromeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

    /// <inheritdoc/>
    protected override IWebDriver CreateDriver(ChromeDriverService service, ChromeOptions options, TimeSpan commandTimeout)
    {
        ChromeDriver driver = new(service, options, commandTimeout);
        ReplaceLocalhostInDebuggerAddress(driver.Capabilities, "goog:chromeOptions");
        return driver;
    }

    /// <summary>
    /// Adds an additional Chrome browser option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public ChromeDriverBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
    {
        Guard.ThrowIfNullOrWhitespace(optionName);

        return WithOptions(options => options.AddAdditionalChromeOption(optionName, optionValue));
    }
}
