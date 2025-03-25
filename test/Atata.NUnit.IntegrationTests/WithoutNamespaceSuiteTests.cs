namespace Atata.NUnit.IntegrationTests;

public sealed class WithoutNamespaceSuiteTests : AtataTestSuite
{
    protected override void ConfigureTestAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable("custom-test-variable", true);

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
        }
    }

    [Test]
    [TestCase(true)]
    public void Context_Test_ForTestCase(bool justFlag)
    {
        using (new AssertionScope())
        {
            string testNameSuffix = $"({justFlag})";
            Context.Test.Name.Should().Be(nameof(Context_Test_ForTestCase) + testNameSuffix);
            Context.Test.SuiteName.Should().Be(nameof(WithoutNamespaceSuiteTests));
            Context.Test.SuiteType.Should().Be<WithoutNamespaceSuiteTests>();
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{nameof(Context_Test_ForTestCase)}{testNameSuffix}");
        }
    }

    [Test]
    public void Context_ParentContext() =>
        Context.ParentContext!.Test.Should().Be(new TestInfo(typeof(WithoutNamespaceSuiteTests)));

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
