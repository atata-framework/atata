using OpenQA.Selenium.Safari;

namespace Atata;

public class SafariDriverBuilder : WebDriverBuilder<SafariDriverBuilder, SafariDriverService, SafariOptions>
{
    public SafariDriverBuilder()
        : base(WebDriverAliases.Safari, "Safari")
    {
    }

    protected override SafariDriverService CreateService() =>
        SafariDriverService.CreateDefaultService();

    protected override SafariDriverService CreateService(string driverPath) =>
        SafariDriverService.CreateDefaultService(driverPath);

    protected override SafariDriverService CreateService(string driverPath, string driverExecutableFileName) =>
        SafariDriverService.CreateDefaultService(driverPath, driverExecutableFileName);

    protected override IWebDriver CreateDriver(SafariDriverService service, SafariOptions options, TimeSpan commandTimeout) =>
        new SafariDriver(service, options, commandTimeout);
}
