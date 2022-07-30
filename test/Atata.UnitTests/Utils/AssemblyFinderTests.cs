using System.Reflection;

namespace Atata.UnitTests.Utils;

[TestFixture]
public class AssemblyFinderTests
{
    [Test]
    public void Find()
    {
        Assembly currentAssembly = Assembly.GetAssembly(typeof(AssemblyFinderTests));

        AssemblyFinder.Find(currentAssembly.GetName().Name)
            .Should().BeSameAs(currentAssembly);
    }

    [Test]
    public void Find_Throws_NotFound()
    {
        Assert.Throws<AssemblyNotFoundException>(() =>
            AssemblyFinder.Find("MissingName"));
    }

    [Test]
    public void FindAllByPattern()
    {
        var items = AssemblyFinder.FindAllByPattern("^Atata$");

        items.Should().ContainSingle()
            .Which.GetName().Name.Should().Be("Atata");
    }

    [Test]
    public void FindAllByPatterns()
    {
        var items = AssemblyFinder.FindAllByPatterns("^Atata$", "^Atata.UnitTests$");

        items.Should().HaveCount(2);
        items.Should().Contain(x => x.GetName().Name == "Atata");
        items.Should().Contain(x => x.GetName().Name == "Atata.UnitTests");
    }
}
