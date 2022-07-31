using System.Reflection;

namespace Atata.UnitTests.Utils;

[TestFixture]
public class TypeFinderTests
{
    private Assembly[] _assembliesToFindIn;

    [OneTimeSetUp]
    public void SetUpFixture()
    {
        _assembliesToFindIn = new[]
        {
            Assembly.GetAssembly(typeof(AtataContext)),
            Assembly.GetAssembly(typeof(TypeFinderTests))
        };
    }

    [TestCase("Atata.UnitTests.TestPage, Atata.UnitTests", ExpectedResult = typeof(TestPage))]
    [TestCase("Atata.UnitTests.TestPage", ExpectedResult = typeof(TestPage))]
    [TestCase("atata.unittests.testpage", ExpectedResult = typeof(TestPage))]
    [TestCase("unittests.testpage", ExpectedResult = typeof(TestPage))]
    [TestCase("TestPage", ExpectedResult = typeof(TestPage))]
    [TestCase("testpage", ExpectedResult = typeof(TestPage))]

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

    [TestCase("button", ExpectedResult = typeof(Button<>))]
    [TestCase("button`1", ExpectedResult = typeof(Button<>))]
    [TestCase("button`2", ExpectedResult = typeof(Button<,>))]

    [TestCase("table", ExpectedResult = typeof(Table<,,>))]
    [TestCase("table`1", ExpectedResult = typeof(Table<>))]
    [TestCase("table`3", ExpectedResult = typeof(Table<,,>))]

    [TestCase("atata.table", ExpectedResult = typeof(Table<,,>))]
    [TestCase("atata.table`2", ExpectedResult = typeof(Table<,>))]
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
