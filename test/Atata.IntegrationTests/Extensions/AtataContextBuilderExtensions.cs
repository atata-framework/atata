namespace Atata.IntegrationTests;

internal static class AtataContextBuilderExtensions
{
    internal static CustomWebDriverBuilder UseFakeDriver(this AtataContextBuilder builder) =>
        builder.UseDriver(FakeWebDriver.Create());
}
