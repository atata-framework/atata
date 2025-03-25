namespace Atata.MSTest.IntegrationTests;

[TestClass]
public sealed class WithoutCustomClassInitializationTests : AtataTestSuite
{
    protected override void ConfigureTestAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable("custom-test-variable", true);

    [TestMethod]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [TestMethod]
    public void Context_ParentContext() =>
        Context.ParentContext!.Test.Should().Be(new TestInfo(typeof(WithoutCustomClassInitializationTests)));

    [TestMethod]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [TestMethod]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"{nameof(WithoutCustomClassInitializationTests)}/{nameof(Context_Artifacts)}");

    [TestMethod]
    public void Context_Variables() =>
        Context.Variables["custom-test-variable"].Should().Be(true);
}
