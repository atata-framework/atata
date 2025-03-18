using Atata.Xunit;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssemblyFixture;

namespace Atata.IntegrationTests.Xunit;

public sealed class WithClassFixtureTests :
    AtataTestSuite,
    IAssemblyFixture<GlobalFixture>,
    IClassFixture<SomeClassFixture<WithClassFixtureTests>>
{
    public WithClassFixtureTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public void Context_IsCurrent() =>
        Context.Should().NotBeNull().And.Be(AtataContext.Current);

    [Fact]
    public void Context_ParentContext() =>
        Context.ParentContext!.Test.Should().Be(new TestInfo(typeof(WithClassFixtureTests)));

    [Fact]
    public void Context_ParentContext_ParentContext() =>
        Context.ParentContext!.ParentContext.Should().NotBeNull().And.Be(AtataContext.Global);

    [Fact]
    public void Context_Variables() =>
        Context.Variables[nameof(SomeClassFixture<WithClassFixtureTests>)].Should().Be(true);

    [Fact]
    public void Context_Artifacts() =>
        Context.ArtifactsRelativePath.Should().Be(
            $"{nameof(WithClassFixtureTests)}/{nameof(Context_Artifacts)}");
}
