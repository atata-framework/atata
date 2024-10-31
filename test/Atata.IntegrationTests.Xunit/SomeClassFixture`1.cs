using Atata.Xunit;

namespace Atata.IntegrationTests.Xunit;

public sealed class SomeClassFixture<TClass> : AtataClassFixture<TClass>
{
    protected override void ConfigureAtataContext(AtataContextBuilder builder)
    {
        base.ConfigureAtataContext(builder);

        builder.AddVariable(nameof(SomeClassFixture<TClass>), true);
    }
}
