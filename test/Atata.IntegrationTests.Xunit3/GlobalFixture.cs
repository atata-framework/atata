using Atata.IntegrationTests.Xunit3;

[assembly: AssemblyFixture(typeof(GlobalFixture))]

namespace Atata.IntegrationTests.Xunit3;

public sealed class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureAtataContextGlobalProperties(AtataContextGlobalProperties globalProperties) =>
        globalProperties.UseRootNamespaceOf<GlobalFixture>();

    protected override void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder) =>
        builder.LogConsumers.AddNLogFile();
}
