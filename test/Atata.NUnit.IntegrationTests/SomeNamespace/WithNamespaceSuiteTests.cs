namespace Atata.NUnit.IntegrationTests.SomeNamespace;

public sealed class WithNamespaceSuiteTests : AtataTestSuite
{
    [Test]
    public void Context_ParentContext_Test() =>
        Context.ParentContext!.Test.Should().Be(new TestInfo(typeof(WithNamespaceSuiteTests)));

    [Test]
    public void Context_ParentContext_ParentContext_Test() =>
        Context.ParentContext!.ParentContext!.Test.Should().Be(
            new TestInfo(
                typeof(NamespaceFixture),
                traits: [
                    new TestTrait(TestTrait.CategoryName, "NUnit namespace category 1")
                ]));

    [Test]
    public void Context_ParentContext_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext!.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Test]
    public void Context_Variables() =>
        Context.Variables[nameof(NamespaceFixture)].Should().Be(true);

    [Test]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"SomeNamespace/{nameof(WithNamespaceSuiteTests)}/{nameof(Context_Artifacts)}");
}
