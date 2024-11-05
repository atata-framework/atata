using Atata.NUnit;

namespace Atata.IntegrationTests.NUnit.SomeNamespace;

public sealed class NamespaceFixture : AtataNamespaceFixture
{
    protected override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        base.ConfigureAtataContext(builder);

        builder.AddVariable(nameof(NamespaceFixture), true);
    }
}
