using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class AssemblyFinderTests
    {
        [Test]
        public void AssemblyFinder_Find()
        {
            Assembly currentAssembly = Assembly.GetAssembly(typeof(AssemblyFinderTests));

            AssemblyFinder.Find(currentAssembly.GetName().Name)
                .Should().BeSameAs(currentAssembly);
        }

        [Test]
        public void AssemblyFinder_Find_Throws_NotFound()
        {
            Assert.Throws<AssemblyNotFoundException>(() =>
                AssemblyFinder.Find("MissingName"));
        }

        [Test]
        public void AssemblyFinder_FindAllByPattern()
        {
            var items = AssemblyFinder.FindAllByPattern("^Atata$");

            items.Should().ContainSingle()
                .Which.GetName().Name.Should().Be("Atata");
        }

        [Test]
        public void AssemblyFinder_FindAllByPatterns()
        {
            var items = AssemblyFinder.FindAllByPatterns("^Atata$", "^Atata.Tests$");

            items.Should().HaveCount(2);
            items.Should().Contain(x => x.GetName().Name == "Atata");
            items.Should().Contain(x => x.GetName().Name == "Atata.Tests");
        }
    }
}
