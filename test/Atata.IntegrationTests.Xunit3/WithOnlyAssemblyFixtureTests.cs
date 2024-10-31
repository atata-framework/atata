using Atata.Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Atata.IntegrationTests.Xunit3;

public sealed class WithOnlyAssemblyFixtureTests : AtataTestSuite
{
    [Fact]
    public void Context_Test_ForFact()
    {
        using (new AssertionScope())
        {
            Context.Test.Name.Should().Be(nameof(Context_Test_ForFact));
            Context.Test.SuiteName.Should().Be(nameof(WithOnlyAssemblyFixtureTests));
            Context.Test.SuiteType.Should().Be(typeof(WithOnlyAssemblyFixtureTests));
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{nameof(Context_Test_ForFact)}");
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
            Context.Test.SuiteType.Should().Be(typeof(WithOnlyAssemblyFixtureTests));
            Context.Test.FullName.Should().Be($"{GetType().FullName}.{nameof(Context_Test_ForTheory)}{testNameSuffix}");
        }
    }

    [Fact]
    public void Context_ParentContext() =>
        Context.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Fact]
    public void Context_Artifacts() =>
        Context.Artifacts.FullName.Value
            .Replace(AtataContext.GlobalProperties.ArtifactsRootPath, null)
            .Should().Be(@$"\{nameof(WithOnlyAssemblyFixtureTests)}\{nameof(Context_Artifacts)}");
}
