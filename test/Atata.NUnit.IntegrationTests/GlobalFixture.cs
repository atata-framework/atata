[assembly: SetCulture("en-US")]
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace Atata.NUnit.IntegrationTests;

public sealed class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties) =>
        globalProperties.UseRootNamespaceOf<GlobalFixture>();

    protected override void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder) =>
        builder.LogConsumers.AddNLogFile();
}
