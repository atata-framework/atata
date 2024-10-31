using Atata.IntegrationTests.Xunit3;
using Atata.Xunit;
using Xunit;

[assembly: AssemblyFixture(typeof(GlobalFixture))]

namespace Atata.IntegrationTests.Xunit3;

public sealed class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureBaseConfiguration(AtataContextBuilder builder) =>
        builder.LogConsumers.AddNLogFile();
}
