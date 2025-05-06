using OpenQA.Selenium.Firefox;

namespace Atata;

/// <summary>
/// Represents a builder for creating and configuring <see cref="FirefoxDriver"/> instances.
/// </summary>
public class FirefoxDriverBuilder : WebDriverBuilder<FirefoxDriverBuilder, FirefoxDriverService, FirefoxOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FirefoxDriverBuilder"/> class.
    /// </summary>
    public FirefoxDriverBuilder()
        : base(WebDriverAliases.Firefox, "Firefox")
    {
    }

    /// <inheritdoc/>
    protected override FirefoxDriverService CreateService()
        => FirefoxDriverService.CreateDefaultService();

    /// <inheritdoc/>
    protected override FirefoxDriverService CreateService(string driverPath)
        => FirefoxDriverService.CreateDefaultService(driverPath);

    /// <inheritdoc/>
    protected override FirefoxDriverService CreateService(string driverPath, string driverExecutableFileName)
        => FirefoxDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

    /// <inheritdoc/>
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
    /// Adds an additional Firefox browser option to the driver options.
    /// </summary>
    /// <param name="optionName">The name of the option to add.</param>
    /// <param name="optionValue">The value of the option to add.</param>
    /// <returns>The same builder instance.</returns>
    public FirefoxDriverBuilder AddAdditionalBrowserOption(string optionName, object optionValue)
    {
        Guard.ThrowIfNullOrWhitespace(optionName);

        return WithOptions(options => options.AddAdditionalFirefoxOption(optionName, optionValue));
    }
}
