using Atata.NUnit;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.IntegrationTests.NUnit.SomeNamespace;

public sealed class WithNamespaceSuiteTests : AtataTestSuite
{
    [Test]
    public void Context_ParentContext() =>
        Context.ParentContext.Test.Should().Be(new TestInfo(typeof(WithNamespaceSuiteTests)));

    [Test]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext.ParentContext.Test.Should().Be(new TestInfo(typeof(NamespaceFixture)));

    [Test]
    public void Context_ParentContext_ParentContext_ParentContext() =>
        Context.ParentContext.ParentContext.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Test]
    public void Context_Variables() =>
        Context.Variables[nameof(NamespaceFixture)].Should().Be(true);

    [Test]
    public void Context_Artifacts() =>
        Context.Artifacts.FullName.Value
            .Replace(AtataContext.GlobalProperties.ArtifactsRootPath, null)
            .Should().Be(@$"{Path.DirectorySeparatorChar}{nameof(WithNamespaceSuiteTests)}{Path.DirectorySeparatorChar}{nameof(Context_Artifacts)}");
}
