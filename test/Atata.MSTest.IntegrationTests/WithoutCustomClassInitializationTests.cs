using FluentAssertions.Execution;

namespace Atata.MSTest.IntegrationTests;

[TestClass]
[TestCategory("MSTest suite category 1")]
[TestProperty("MSTest suite trait 1", "x")]
public sealed class WithoutCustomClassInitializationTests : AtataTestSuite
{
    protected override void ConfigureTestAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable("custom-test-variable", true);

    [TestMethod]
    [TestCategory("MSTest test category 1")]
    [TestProperty("MSTest test trait 1", "x")]
    [TestProperty("MSTest test trait 2", "y")]
    public void Context_Test_Traits() =>
        Context.Test.Traits.Should().Equal(
            new TestTrait(TestTrait.CategoryName, "MSTest test category 1"),
            new TestTrait("MSTest test trait 1", "x"),
            new TestTrait("MSTest test trait 2", "y"));

    [TestMethod]
    public void Context_ParentContext_Test() =>
        Context.ParentContext!.Test.Should().Be(
            new TestInfo(
                typeof(WithoutCustomClassInitializationTests),
                traits: [
                    new TestTrait(TestTrait.CategoryName, "MSTest suite category 1"),
                    new TestTrait("MSTest suite trait 1", "x")
                ]));

    [TestMethod]
    public void Context_Test_ForTest()
    {
        using (new AssertionScope())
        {
            Context.Test.Name.Should().Be(nameof(Context_Test_ForTest));
            Context.Test.SuiteName.Should().Be(nameof(WithoutCustomClassInitializationTests));
            Context.Test.SuiteType.Should().Be<WithoutCustomClassInitializationTests>();
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{nameof(Context_Test_ForTest)}");
            Context.Test.Traits.Should().BeEmpty();
        }
    }

    [TestMethod]
    [DataRow(true)]
    public void Context_Test_ForTestCase(bool justFlag)
    {
        using (new AssertionScope())
        {
            string expectedTestName = $"{nameof(Context_Test_ForTestCase)} ({justFlag})";
            Context.Test.Name.Should().Be(expectedTestName);
            Context.Test.SuiteName.Should().Be(nameof(WithoutCustomClassInitializationTests));
            Context.Test.SuiteType.Should().Be<WithoutCustomClassInitializationTests>();
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{expectedTestName}");
            Context.Test.Traits.Should().BeEmpty();
        }
    }

    [TestMethod]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

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
