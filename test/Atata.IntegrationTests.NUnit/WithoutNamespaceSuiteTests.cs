using Atata.NUnit;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Atata.IntegrationTests.NUnit;

public sealed class WithoutNamespaceSuiteTests : AtataTestSuite
{
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
        Context.ParentContext.Test.Should().Be(new TestInfo(typeof(WithoutNamespaceSuiteTests)));

    [Test]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Test]
    public void Context_Artifacts() =>
        Context.Artifacts.FullName.Value
            .Replace(AtataContext.GlobalProperties.ArtifactsRootPath, null)
            .Should().Be(@$"{Path.DirectorySeparatorChar}{nameof(WithoutNamespaceSuiteTests)}{Path.DirectorySeparatorChar}{nameof(Context_Artifacts)}");
}
