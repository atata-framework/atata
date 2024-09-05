namespace Atata.IntegrationTests;

internal static class WebDriverSessionBuilderExtensions
{
    internal static CustomWebDriverBuilder UseFakeDriver(this WebDriverSessionBuilder builder) =>
        builder.UseDriver(FakeWebDriver.Create());
}
