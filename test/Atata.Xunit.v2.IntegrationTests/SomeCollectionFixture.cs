namespace Atata.Xunit.IntegrationTests;

public sealed class SomeCollectionFixture : AtataCollectionFixture
{
    public SomeCollectionFixture()
        : base(SomeCollection.Name)
    {
    }

    protected override void ConfigureCollectionAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable(nameof(SomeCollectionFixture), true);
}
