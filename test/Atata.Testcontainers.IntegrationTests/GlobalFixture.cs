[assembly: SetCulture("en-US")]
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace Atata.Testcontainers.IntegrationTests;

public sealed class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties) =>
        globalProperties.UseRootNamespaceOf<GlobalFixture>();
}
