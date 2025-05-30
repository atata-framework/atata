namespace Atata.Xunit.IntegrationTests;

[Collection(SomeCollection.Name)]
public sealed class WithCollectionAndClassFixturesTests :
    AtataTestSuite,
    IClassFixture<SomeClassFixture<WithCollectionAndClassFixturesTests>>
{
    [Fact]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [Fact]
    public void Context_ParentContext() =>
        Context.ParentContext!.Test.Should().Be(
            new TestInfo(typeof(WithCollectionAndClassFixturesTests), suiteGroupName: SomeCollection.Name));

    [Fact]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext!.Test.Should().NotBeNull().And.Be(
            new TestInfo(typeof(SomeCollectionFixture), suiteGroupName: SomeCollection.Name));

    [Fact]
    public void Context_ParentContext_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext!.ParentContext.Should().NotBeNull().And.Be(
            AtataContext.Global);

    [Fact]
    public void Context_Variables() =>
        Context.Variables[nameof(SomeCollectionFixture)].Should().Be(true);

    [Fact]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"{nameof(WithCollectionAndClassFixturesTests)}/{nameof(Context_Artifacts)}");
}
