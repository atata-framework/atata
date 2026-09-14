namespace Atata.UnitTests;

public sealed class AttributesBuilderTests
{
    private readonly Attribute[] _stubAttributes =
    [
        new CultureAttribute("en-US"),
        new TermAttribute("some-id")
    ];

    private AttributesBuilder _sut = null!;

    [SetUp]
    public void SetUp() =>
        _sut = new(AtataContext.CreateDefaultNonScopedBuilder(), new());

    [Test]
    public void Global()
    {
        _sut.Global
            .Add(_stubAttributes);

        _sut.AttributesContext.Global
            .Should().Equal(_stubAttributes);
    }

    [Test]
    public void Assembly_ByAssemblyName()
    {
        _sut.Assembly("Atata")
            .Add(_stubAttributes);

        _sut.AttributesContext.AssemblyMap.Keys.First()
            .GetName().Name.Should().Be("Atata");

        _sut.AttributesContext.AssemblyMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }

    [Test]
    public void Assembly_ByAssembly()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        _sut.Assembly(assembly)
            .Add(_stubAttributes);

        _sut.AttributesContext.AssemblyMap.Keys.First()
            .Should().BeSameAs(assembly);

        _sut.AttributesContext.AssemblyMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }

    [Test]
    public void Component_ByGenericParameter()
    {
        _sut.Component<TestComponent>()
            .Add(_stubAttributes);

        _sut.AttributesContext.ComponentMap.Keys.First()
            .Should().Be<TestComponent>();

        _sut.AttributesContext.ComponentMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }

    [Test]
    public void Component_ByType()
    {
        _sut.Component(typeof(TestComponent))
            .Add(_stubAttributes);

        _sut.AttributesContext.ComponentMap.Keys.First()
            .Should().Be<TestComponent>();

        _sut.AttributesContext.ComponentMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }

    [TestCase("Atata.UnitTests." + nameof(TestComponent) + ", Atata.UnitTests")]
    [TestCase("Atata.UnitTests." + nameof(TestComponent))]
    [TestCase(nameof(TestComponent))]
    public void Component_ByTypeName(string typeName)
    {
        _sut.Component(typeName)
            .Add(_stubAttributes);

        _sut.AttributesContext.ComponentMap.Keys.First()
            .Should().Be<TestComponent>();

        _sut.AttributesContext.ComponentMap.Values
            .Should().ContainSingle().Which
            .Should().Equal(_stubAttributes);
    }
}
