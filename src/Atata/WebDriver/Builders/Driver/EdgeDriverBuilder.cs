using OpenQA.Selenium.Edge;

namespace Atata;

/// <summary>
/// Represents a builder for creating and configuring <see cref="EdgeDriver"/> instances.
/// </summary>
public class EdgeDriverBuilder : ChromiumDriverBuilder<EdgeDriverBuilder, EdgeDriverService, EdgeOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeDriverBuilder"/> class.
    /// </summary>
    public EdgeDriverBuilder()
        : base(WebDriverAliases.Edge, "Edge")
    {
    }

    /// <inheritdoc/>
    protected override EdgeDriverService CreateService() =>
        EdgeDriverService.CreateDefaultService();

    /// <inheritdoc/>
    protected override EdgeDriverService CreateService(string driverPath) =>
        EdgeDriverService.CreateDefaultService(driverPath);

    /// <inheritdoc/>
    protected override EdgeDriverService CreateService(string driverPath, string driverExecutableFileName) =>
        EdgeDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

    /// <inheritdoc/>
    protected override IWebDriver CreateDriver(EdgeDriverService service, EdgeOptions options, TimeSpan commandTimeout)
    {
        EdgeDriver driver = new(service, options, commandTimeout);
        ReplaceLocalhostInDebuggerAddress(driver.Capabilities, "ms:edgeOptions");
        return driver;
    }

    /// <summary>
    /// Adds an additional Edge browser option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public EdgeDriverBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
    {
        Guard.ThrowIfNullOrWhitespace(optionName);

        return WithOptions(options => options.AddAdditionalEdgeOption(optionName, optionValue));
    }
}
