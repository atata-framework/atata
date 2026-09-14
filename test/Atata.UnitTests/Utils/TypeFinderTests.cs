namespace Atata.UnitTests.Utils;

public sealed class TypeFinderTests
{
    private Assembly[] _assembliesToFindIn = null!;

    [OneTimeSetUp]
    public void SetUpFixture() =>
        _assembliesToFindIn =
        [
            Assembly.GetAssembly(typeof(AtataContext))!,
            Assembly.GetAssembly(typeof(TypeFinderTests))!
        ];

    [TestCase("Atata.UnitTests.TestComponent, Atata.UnitTests", ExpectedResult = typeof(TestComponent))]
    [TestCase("Atata.UnitTests.TestComponent", ExpectedResult = typeof(TestComponent))]
    [TestCase("atata.unittests.testcomponent", ExpectedResult = typeof(TestComponent))]
    [TestCase("unittests.testcomponent", ExpectedResult = typeof(TestComponent))]
    [TestCase("TestComponent", ExpectedResult = typeof(TestComponent))]
    [TestCase("testcomponent", ExpectedResult = typeof(TestComponent))]

    [TestCase("Atata.UnitTests.Utils.TypeFinderTests+SubClass, Atata.UnitTests", ExpectedResult = typeof(SubClass))]
    [TestCase("Atata.UnitTests.Utils.TypeFinderTests+SubClass", ExpectedResult = typeof(SubClass))]
    [TestCase("atata.unittests.utils.typefindertests+subclass", ExpectedResult = typeof(SubClass))]
    [TestCase("utils.typefindertests+subclass", ExpectedResult = typeof(SubClass))]
    [TestCase("TypeFinderTests+SubClass", ExpectedResult = typeof(SubClass))]
    [TestCase("typefindertests+subclass", ExpectedResult = typeof(SubClass))]
    [TestCase("SubClass", ExpectedResult = typeof(SubClass))]
    [TestCase("subclass", ExpectedResult = typeof(SubClass))]
    [TestCase("StaticSubClass+NonGenericSubClass", ExpectedResult = typeof(StaticSubClass.NonGenericSubClass))]

    [TestCase("Atata.UnitTests.Utils.TypeFinderTests+StaticSubClass+GenericSubClass", ExpectedResult = typeof(StaticSubClass.GenericSubClass<>))]
    [TestCase("Atata.UnitTests.Utils.TypeFinderTests+StaticSubClass+GenericSubClass`1", ExpectedResult = typeof(StaticSubClass.GenericSubClass<>))]
    [TestCase("Utils.TypeFinderTests+StaticSubClass+GenericSubClass`1", ExpectedResult = typeof(StaticSubClass.GenericSubClass<>))]

    [TestCase("testcomponent", ExpectedResult = typeof(TestComponent))]
    [TestCase("testcomponent`1", ExpectedResult = typeof(TestComponent<>))]
    [TestCase("testcomponent`2", ExpectedResult = typeof(TestComponent<,>))]

    [TestCase("atata.unittests.testcomponent", ExpectedResult = typeof(TestComponent))]
    [TestCase("atata.unittests.testcomponent`1", ExpectedResult = typeof(TestComponent<>))]
    public Type FindInAssemblies_WithKnownTypeName(string typeName) =>
        TypeFinder.FindInAssemblies(typeName, _assembliesToFindIn);

    [TestCase("Atata.UnitTests.MissingType, Atata.UnitTests")]
    [TestCase("Atata.UnitTests.MissingType")]
    [TestCase("MissingType")]
    [TestCase("Button`3")]
    [TestCase("Atata1.Button`1")]
    public void FindInAssemblies_WithUnknownTypeName(string typeName) =>
        Assert.Throws<TypeNotFoundException>(() =>
            TypeFinder.FindInAssemblies(typeName, _assembliesToFindIn));

    public static class StaticSubClass
    {
        public class NonGenericSubClass
        {
        }

        public class GenericSubClass<T>
        {
        }
    }

    public class SubClass
    {
    }
}
