namespace Atata.NUnit.IntegrationTests.SomeNamespace;

public sealed class NamespaceFixture : AtataNamespaceFixture
{
    protected override void ConfigureNamespaceAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable(nameof(NamespaceFixture), true);
}
