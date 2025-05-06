using OpenQA.Selenium.Safari;

namespace Atata;

/// <summary>
/// Represents a builder for creating and configuring <see cref="SafariDriver"/> instances.
/// </summary>
public class SafariDriverBuilder : WebDriverBuilder<SafariDriverBuilder, SafariDriverService, SafariOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SafariDriverBuilder"/> class.
    /// </summary>
    public SafariDriverBuilder()
        : base(WebDriverAliases.Safari, "Safari")
    {
    }

    /// <inheritdoc/>
    protected override SafariDriverService CreateService() =>
        SafariDriverService.CreateDefaultService();

    /// <inheritdoc/>
    protected override SafariDriverService CreateService(string driverPath) =>
        SafariDriverService.CreateDefaultService(driverPath);

    /// <inheritdoc/>
    protected override SafariDriverService CreateService(string driverPath, string driverExecutableFileName) =>
        SafariDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

    /// <inheritdoc/>
    protected override IWebDriver CreateDriver(SafariDriverService service, SafariOptions options, TimeSpan commandTimeout) =>
        new SafariDriver(service, options, commandTimeout);
}
