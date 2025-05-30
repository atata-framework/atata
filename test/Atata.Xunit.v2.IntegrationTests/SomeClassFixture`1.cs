namespace Atata.Xunit.IntegrationTests;

public sealed class SomeClassFixture<TClass> : AtataClassFixture<TClass>
{
    protected override void ConfigureSuiteAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable(nameof(SomeClassFixture<TClass>), true);
}
