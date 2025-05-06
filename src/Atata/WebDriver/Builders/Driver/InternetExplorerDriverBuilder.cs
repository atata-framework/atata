using OpenQA.Selenium.IE;

namespace Atata;

/// <summary>
/// Represents a builder for creating and configuring <see cref="InternetExplorerDriver"/> instances.
/// </summary>
public class InternetExplorerDriverBuilder : WebDriverBuilder<InternetExplorerDriverBuilder, InternetExplorerDriverService, InternetExplorerOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InternetExplorerDriverBuilder"/> class.
    /// </summary>
    public InternetExplorerDriverBuilder()
        : base(WebDriverAliases.InternetExplorer, "Internet Explorer")
    {
    }

    /// <inheritdoc/>
    protected override InternetExplorerDriverService CreateService() =>
        InternetExplorerDriverService.CreateDefaultService();

    /// <inheritdoc/>
    protected override InternetExplorerDriverService CreateService(string driverPath) =>
        InternetExplorerDriverService.CreateDefaultService(driverPath);

    /// <inheritdoc/>
    protected override InternetExplorerDriverService CreateService(string driverPath, string driverExecutableFileName) =>
        InternetExplorerDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

    /// <inheritdoc/>
    protected override IWebDriver CreateDriver(InternetExplorerDriverService service, InternetExplorerOptions options, TimeSpan commandTimeout) =>
        new InternetExplorerDriver(service, options, commandTimeout);

    /// <summary>
    /// Adds an additional Internet Explorer browser option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public InternetExplorerDriverBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
    {
        Guard.ThrowIfNullOrWhitespace(optionName);

        return WithOptions(options => options.AddAdditionalInternetExplorerOption(optionName, optionValue));
    }
}
