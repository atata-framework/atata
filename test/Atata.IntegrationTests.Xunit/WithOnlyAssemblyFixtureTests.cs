using Atata.Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssemblyFixture;

namespace Atata.IntegrationTests.Xunit;

public sealed class WithOnlyAssemblyFixtureTests :
    AtataTestSuite,
    IAssemblyFixture<GlobalFixture>
{
    public WithOnlyAssemblyFixtureTests(ITestOutputHelper output)
        : base(output)
    {
    }

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
            .Should().Be(@$"{Path.DirectorySeparatorChar}{nameof(WithOnlyAssemblyFixtureTests)}{Path.DirectorySeparatorChar}{nameof(Context_Artifacts)}");
}
