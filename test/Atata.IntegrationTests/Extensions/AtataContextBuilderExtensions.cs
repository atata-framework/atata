namespace Atata.IntegrationTests;

internal static class AtataContextBuilderExtensions
{
    internal static CustomDriverAtataContextBuilder UseFakeDriver(this AtataContextBuilder builder) =>
        builder.UseDriver(FakeWebDriver.Create());
}
