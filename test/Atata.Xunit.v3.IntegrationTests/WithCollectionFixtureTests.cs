namespace Atata.Xunit.IntegrationTests;

[Collection(SomeCollection.Name)]
public sealed class WithCollectionFixtureTests : AtataTestSuite
{
    [Fact]
    [Trait("Xunit test trait 1", "x")]
    [Trait("Xunit test trait 2", "y")]
    public void Context_Test_Traits() =>
        Context.Test.Traits.Should().Equal(
            new TestTrait("Xunit collection trait 1", "x"),
            new TestTrait("Xunit test trait 1", "x"),
            new TestTrait("Xunit test trait 2", "y"));

    [Fact]
    public void Context_ParentContext_Test() =>
        Context.ParentContext!.Test.Should().Be(
            new TestInfo(typeof(SomeCollectionFixture), suiteGroupName: SomeCollection.Name));

    [Fact]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [Fact]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Fact]
    public void Context_Variables() =>
        Context.Variables[nameof(SomeCollectionFixture)].Should().Be(true);

    [Fact]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"{nameof(WithCollectionFixtureTests)}/{nameof(Context_Artifacts)}");
}
