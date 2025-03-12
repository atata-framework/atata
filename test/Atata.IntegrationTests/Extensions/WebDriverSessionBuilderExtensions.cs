#nullable enable

namespace Atata.IntegrationTests;

internal static class WebDriverSessionBuilderExtensions
{
    internal static WebDriverSessionBuilder UseFakeDriver(
        this WebDriverSessionBuilder builder,
        Action<CustomWebDriverBuilder>? configure = null)
        =>
        builder.UseDriver(FakeWebDriver.Create(), configure);
}
