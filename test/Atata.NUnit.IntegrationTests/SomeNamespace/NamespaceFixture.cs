namespace Atata.NUnit.IntegrationTests.SomeNamespace;

[Category("NUnit namespace category 1")]
public sealed class NamespaceFixture : AtataNamespaceFixture
{
    protected override void ConfigureNamespaceAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable(nameof(NamespaceFixture), true);
}
