namespace Atata.Xunit.IntegrationTests;

public sealed class WithClassFixtureTests :
    AtataTestSuite,
    IClassFixture<SomeClassFixture<WithClassFixtureTests>>
{
    [Fact]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [Fact]
    public void Context_ParentContext() =>
        Context.ParentContext!.Test.Should().Be(new TestInfo(typeof(WithClassFixtureTests)));

    [Fact]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Fact]
    public void Context_Variables() =>
        Context.Variables[nameof(SomeClassFixture<WithClassFixtureTests>)].Should().Be(true);

    [Fact]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"{nameof(WithClassFixtureTests)}/{nameof(Context_Artifacts)}");
}
