using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework.Internal;

namespace Atata.NUnit.IntegrationTests;

[Category("NUnit suite category 1")]
[Property("NUnit suite trait 1", "x")]
public sealed class WithoutNamespaceSuiteTests : AtataTestSuite
{
    protected override void ConfigureTestAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable("custom-test-variable", true);

    [Test]
    [Category("NUnit test category 1")]
    [Property("NUnit test trait 1", "x")]
    [Property("NUnit test trait 2", 7)]
    [Description("Some description")]
    [Author("Some author")]
    public void Context_Test_Traits() =>
        Context.Test.Traits.Should().Equal(
            new TestTrait(TestTrait.CategoryName, "NUnit test category 1"),
            new TestTrait("NUnit test trait 1", "x"),
            new TestTrait("NUnit test trait 2", "7"));

    [Test]
    public void Context_ParentContext_Test() =>
        Context.ParentContext!.Test.Should().Be(
            new TestInfo(
                typeof(WithoutNamespaceSuiteTests),
                traits: [
                    new TestTrait(TestTrait.CategoryName, "NUnit suite category 1"),
                    new TestTrait("NUnit suite trait 1", "x")
                ]));

    [Test]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [Test]
    public void Context_Test_ForTest()
    {
        using (new AssertionScope())
        {
            Context.Test.Name.Should().Be(nameof(Context_Test_ForTest));
            Context.Test.SuiteName.Should().Be(nameof(WithoutNamespaceSuiteTests));
            Context.Test.SuiteType.Should().Be<WithoutNamespaceSuiteTests>();
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{nameof(Context_Test_ForTest)}");
            Context.Test.Traits.Should().BeEmpty();
        }
    }

    [TestCase(true)]
    public void Context_Test_ForTestCase(bool justFlag)
    {
        using (new AssertionScope())
        {
            string expectedTestName = $"{nameof(Context_Test_ForTestCase)}({justFlag})";
            Context.Test.Name.Should().Be(expectedTestName);
            Context.Test.SuiteName.Should().Be(nameof(WithoutNamespaceSuiteTests));
            Context.Test.SuiteType.Should().Be<WithoutNamespaceSuiteTests>();
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{expectedTestName}");
            Context.Test.Traits.Should().BeEmpty();
        }
    }

    [Test]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Test]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"{nameof(WithoutNamespaceSuiteTests)}/{nameof(Context_Artifacts)}");

    [Test]
    public void Context_Variables() =>
        Context.Variables["custom-test-variable"].Should().Be(true);
}
