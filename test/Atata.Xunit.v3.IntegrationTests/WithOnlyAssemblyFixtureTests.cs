namespace Atata.Xunit.IntegrationTests;

[Trait("Xunit suite trait 1", "x")]
public sealed class WithOnlyAssemblyFixtureTests : AtataTestSuite
{
    protected override void ConfigureTestAtataContext(AtataContextBuilder builder) =>
        builder.UseVariable("custom-test-variable", true);

    [Fact]
    [Trait("Xunit test trait 1", "x")]
    [Trait("Xunit test trait 2", "y")]
    public void Context_Test_Traits() =>
        Context.Test.Traits.Should().Equal(
            new TestTrait("Xunit suite trait 1", "x"),
            new TestTrait("Xunit test trait 1", "x"),
            new TestTrait("Xunit test trait 2", "y"));

    [Fact]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [Fact]
    public void Context_Test_ForFact()
    {
        using (new AssertionScope())
        {
            Context.Test.Name.Should().Be(nameof(Context_Test_ForFact));
            Context.Test.SuiteName.Should().Be(nameof(WithOnlyAssemblyFixtureTests));
            Context.Test.SuiteType.Should().Be<WithOnlyAssemblyFixtureTests>();
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{nameof(Context_Test_ForFact)}");
            Context.Test.Traits.Should().Equal(new TestTrait("Xunit suite trait 1", "x"));
        }
    }

    [Theory]
    [InlineData(true)]
    public void Context_Test_ForTheory(bool justFlag)
    {
        using (new AssertionScope())
        {
            string testNameSuffix = $"({nameof(justFlag)}: {justFlag})";
            Context.Test.Name.Should().Be(nameof(Context_Test_ForTheory) + testNameSuffix);
            Context.Test.SuiteName.Should().Be(nameof(WithOnlyAssemblyFixtureTests));
            Context.Test.SuiteType.Should().Be<WithOnlyAssemblyFixtureTests>();
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{nameof(Context_Test_ForTheory)}{testNameSuffix}");
        }
    }

    [Fact]
    public void Context_ParentContext() =>
        Context.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Fact]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"{nameof(WithOnlyAssemblyFixtureTests)}/{nameof(Context_Artifacts)}");

    [Fact]
    public void Context_Variables() =>
        Context.Variables["custom-test-variable"].Should().Be(true);
}
