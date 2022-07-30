using System.Reflection;

namespace Atata.UnitTests;

[TestFixture]
public class AttributesAtataContextBuilderTests
{
    private readonly Attribute[] _stubAttributes = new Attribute[]
    {
        new FindByIdAttribute(),
        new TermAttribute("some-id")
    };

    private AttributesAtataContextBuilder _sut;

    [SetUp]
    public void SetUp() =>
        _sut = new AttributesAtataContextBuilder(new AtataBuildingContext());

    [Test]
    public void AttributesAtataContextBuilder_Global()
    {
        _sut.Global
            .Add(_stubAttributes);

        _sut.BuildingContext.Attributes.Global
            .Should().Equal(_stubAttributes);
    }

    [Test]
    public void AttributesAtataContextBuilder_Assembly_ByAssemblyName()
    {
        _sut.Assembly("Atata")
            .Add(_stubAttributes);

        _sut.BuildingContext.Attributes.AssemblyMap.Keys.First()
            .GetName().Name.Should().Be("Atata");

        _sut.BuildingContext.Attributes.AssemblyMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }

    [Test]
    public void AttributesAtataContextBuilder_Assembly_ByAssembly()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        _sut.Assembly(assembly)
            .Add(_stubAttributes);

        _sut.BuildingContext.Attributes.AssemblyMap.Keys.First()
            .Should().BeSameAs(assembly);

        _sut.BuildingContext.Attributes.AssemblyMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }

    [Test]
    public void AttributesAtataContextBuilder_Component_ByGenericParameter()
    {
        _sut.Component<TestPage>()
            .Add(_stubAttributes);

        _sut.BuildingContext.Attributes.ComponentMap.Keys.First()
            .Should().Be(typeof(TestPage));

        _sut.BuildingContext.Attributes.ComponentMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }

    [Test]
    public void AttributesAtataContextBuilder_Component_ByType()
    {
        _sut.Component(typeof(TestPage))
            .Add(_stubAttributes);

        _sut.BuildingContext.Attributes.ComponentMap.Keys.First()
            .Should().Be(typeof(TestPage));

        _sut.BuildingContext.Attributes.ComponentMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }

    [TestCase("Atata.UnitTests." + nameof(TestPage) + ", Atata.UnitTests")]
    [TestCase("Atata.UnitTests." + nameof(TestPage))]
    [TestCase(nameof(TestPage))]
    public void AttributesAtataContextBuilder_Component_ByTypeName(string typeName)
    {
        _sut.Component(typeName)
            .Add(_stubAttributes);

        _sut.BuildingContext.Attributes.ComponentMap.Keys.First()
            .Should().Be(typeof(TestPage));

        _sut.BuildingContext.Attributes.ComponentMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }
}
