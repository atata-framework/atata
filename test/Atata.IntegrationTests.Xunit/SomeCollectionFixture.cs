using Atata.Xunit;

namespace Atata.IntegrationTests.Xunit;

public sealed class SomeCollectionFixture : AtataCollectionFixture
{
    public SomeCollectionFixture()
        : base(SomeCollection.Name)
    {
    }

    protected override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        base.ConfigureAtataContext(builder);

        builder.SetVariable(nameof(SomeCollectionFixture), true);
    }
}
