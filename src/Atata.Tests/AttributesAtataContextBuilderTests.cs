using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class AttributesAtataContextBuilderTests
    {
        private readonly Attribute[] stubAttributes = new Attribute[]
        {
            new FindByIdAttribute(),
            new TermAttribute("some-id")
        };

        private AttributesAtataContextBuilder sut;

        [SetUp]
        public void SetUp()
        {
            sut = new AttributesAtataContextBuilder(new AtataBuildingContext());
        }

        [Test]
        public void AttributesAtataContextBuilder_Global()
        {
            sut.Global
                .Add(stubAttributes);

            sut.BuildingContext.Attributes.Global
                .Should().Equal(stubAttributes);
        }

        [Test]
        public void AttributesAtataContextBuilder_Assembly_ByAssemblyName()
        {
            sut.Assembly("Atata")
                .Add(stubAttributes);

            sut.BuildingContext.Attributes.AssemblyMap.Keys.First()
                .GetName().Name.Should().Be("Atata");

            sut.BuildingContext.Attributes.AssemblyMap.Values
                .Should().ContainSingle().Which
                .Should().Equal(stubAttributes);
        }

        [Test]
        public void AttributesAtataContextBuilder_Assembly_ByAssembly()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            sut.Assembly(assembly)
                .Add(stubAttributes);

            sut.BuildingContext.Attributes.AssemblyMap.Keys.First()
                .Should().BeSameAs(assembly);

            sut.BuildingContext.Attributes.AssemblyMap.Values
                .Should().ContainSingle().Which
                .Should().Equal(stubAttributes);
        }

        [Test]
        public void AttributesAtataContextBuilder_Component_ByGenericParameter()
        {
            sut.Component<StubPage>()
                .Add(stubAttributes);

            sut.BuildingContext.Attributes.ComponentMap.Keys.First()
                .Should().Be(typeof(StubPage));

            sut.BuildingContext.Attributes.ComponentMap.Values
                .Should().ContainSingle().Which
                .Should().Equal(stubAttributes);
        }

        [Test]
        public void AttributesAtataContextBuilder_Component_ByType()
        {
            sut.Component(typeof(StubPage))
                .Add(stubAttributes);

            sut.BuildingContext.Attributes.ComponentMap.Keys.First()
                .Should().Be(typeof(StubPage));

            sut.BuildingContext.Attributes.ComponentMap.Values
                .Should().ContainSingle().Which
                .Should().Equal(stubAttributes);
        }

        [TestCase("Atata.Tests." + nameof(StubPage) + ", Atata.Tests")]
        [TestCase("Atata.Tests." + nameof(StubPage))]
        [TestCase(nameof(StubPage))]
        public void AttributesAtataContextBuilder_Component_ByTypeName(string typeName)
        {
            sut.Component(typeName)
                .Add(stubAttributes);

            sut.BuildingContext.Attributes.ComponentMap.Keys.First()
                .Should().Be(typeof(StubPage));

            sut.BuildingContext.Attributes.ComponentMap.Values
                .Should().ContainSingle().Which
                .Should().Equal(stubAttributes);
        }
    }
}
