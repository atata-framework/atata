namespace Atata.Xunit3.IntegrationTests;

[Collection(SomeCollection.Name)]
public sealed class WithCollectionFixtureTests : AtataTestSuite
{
    [Fact]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [Fact]
    public void Context_ParentContext() =>
        Context.ParentContext!.Test.Should().Be(
            new TestInfo(typeof(SomeCollectionFixture), suiteGroupName: SomeCollection.Name));

    [Fact]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext.Should().NotBeNull().And.Be(
            AtataContext.Global);

    [Fact]
    public void Context_Variables() =>
        Context.Variables[nameof(SomeCollectionFixture)].Should().Be(true);

    [Fact]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"{nameof(WithCollectionFixtureTests)}/{nameof(Context_Artifacts)}");
}
