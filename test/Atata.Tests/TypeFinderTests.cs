using System;
using System.Reflection;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class TypeFinderTests
    {
        private Assembly[] assembliesToFindIn;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            assembliesToFindIn = new[]
            {
                Assembly.GetAssembly(typeof(AtataContext)),
                Assembly.GetAssembly(typeof(TypeFinderTests))
            };
        }

        [TestCase("Atata.Tests.StubPage, Atata.Tests", ExpectedResult = typeof(StubPage))]
        [TestCase("Atata.Tests.StubPage", ExpectedResult = typeof(StubPage))]
        [TestCase("atata.tests.stubpage", ExpectedResult = typeof(StubPage))]
        [TestCase("tests.stubpage", ExpectedResult = typeof(StubPage))]
        [TestCase("StubPage", ExpectedResult = typeof(StubPage))]
        [TestCase("stubpage", ExpectedResult = typeof(StubPage))]

        [TestCase("Atata.Tests.TypeFinderTests+SubClass, Atata.Tests", ExpectedResult = typeof(SubClass))]
        [TestCase("Atata.Tests.TypeFinderTests+SubClass", ExpectedResult = typeof(SubClass))]
        [TestCase("atata.tests.typefindertests+subclass", ExpectedResult = typeof(SubClass))]
        [TestCase("tests.typefindertests+subclass", ExpectedResult = typeof(SubClass))]
        [TestCase("TypeFinderTests+SubClass", ExpectedResult = typeof(SubClass))]
        [TestCase("typefindertests+subclass", ExpectedResult = typeof(SubClass))]
        [TestCase("SubClass", ExpectedResult = typeof(SubClass))]
        [TestCase("subclass", ExpectedResult = typeof(SubClass))]
        [TestCase("StaticSubClass+SubClass", ExpectedResult = typeof(StaticSubClass.SubClass))]

        [TestCase("Atata.Tests.TypeFinderTests+StaticSubClass+InnerSubClass", ExpectedResult = typeof(StaticSubClass.InnerSubClass<>))]
        [TestCase("Atata.Tests.TypeFinderTests+StaticSubClass+InnerSubClass`1", ExpectedResult = typeof(StaticSubClass.InnerSubClass<>))]
        [TestCase("Tests.TypeFinderTests+StaticSubClass+InnerSubClass`1", ExpectedResult = typeof(StaticSubClass.InnerSubClass<>))]

        [TestCase("button", ExpectedResult = typeof(Button<>))]
        [TestCase("button`1", ExpectedResult = typeof(Button<>))]
        [TestCase("button`2", ExpectedResult = typeof(Button<,>))]

        [TestCase("table", ExpectedResult = typeof(Table<,,>))]
        [TestCase("table`1", ExpectedResult = typeof(Table<>))]
        [TestCase("table`3", ExpectedResult = typeof(Table<,,>))]

        [TestCase("atata.table", ExpectedResult = typeof(Table<,,>))]
        [TestCase("atata.table`2", ExpectedResult = typeof(Table<,>))]
        public Type TypeFinder_FindInAssemblies(string typeName)
        {
            return TypeFinder.FindInAssemblies(typeName, assembliesToFindIn);
        }

        [TestCase("Atata.Tests.MissingType, Atata.Tests")]
        [TestCase("Atata.Tests.MissingType")]
        [TestCase("MissingType")]
        [TestCase("Button`3")]
        [TestCase("Atata1.Button`1")]
        public void TypeFinder_FindInAssemblies_Throws_NotFound(string typeName)
        {
            Assert.Throws<TypeNotFoundException>(() =>
                TypeFinder.FindInAssemblies(typeName, assembliesToFindIn));
        }

        public static class StaticSubClass
        {
            public class SubClass
            {
            }

            public class InnerSubClass<T>
            {
            }
        }

        public class SubClass
        {
        }
    }
}
