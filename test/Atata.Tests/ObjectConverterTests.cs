using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class ObjectConverterTests
    {
        private ObjectConverter sut;

        [SetUp]
        public void SetUp()
        {
            sut = new ObjectConverter();
        }

        [Test]
        public void Convert_NullToString()
        {
            sut.Convert(null, typeof(string))
                .Should().BeNull();
        }

        [Test]
        public void Convert_NullToArrayOfInt()
        {
            sut.Convert(null, typeof(int[]))
                .Should().BeNull();
        }

        [Test]
        public void Convert_NullToInt_Throws()
        {
            Assert.Throws<ConversionException>(() =>
                sut.Convert(null, typeof(int)));
        }

        [Test]
        public void Convert_StringToDecimal()
        {
            TestConvert<decimal>("5.555")
                .Should().Be(5.555m);
        }

        [Test]
        public void Convert_DecimalToString()
        {
            TestConvert<string>(5.555m)
                .Should().Be("5.555");
        }

        [Test]
        public void Convert_StringToBool()
        {
            TestConvert<bool>("true")
                .Should().Be(true);
        }

        [Test]
        public void Convert_StringToNullableBool()
        {
            TestConvert<bool?>("true")
                .Should().Be(true);
        }

        [Test]
        public void Convert_BoolToString()
        {
            TestConvert<string>(true)
                .Should().Be("True");
        }

        [Test]
        public void Convert_StringToEnum()
        {
            TestConvert<TermCase>(nameof(TermCase.Kebab))
                .Should().Be(TermCase.Kebab);
        }

        [TestCase("findByIdAttribute", ExpectedResult = typeof(FindByIdAttribute))]
        [TestCase("ordinaryPage", ExpectedResult = typeof(OrdinaryPage))]
        [TestCase("inputPage", ExpectedResult = typeof(InputPage))]
        public Type Convert_StringToType(string value)
        {
            return TestConvert<Type>(value);
        }

        [Test]
        public void Convert_StringToArrayOfString()
        {
            TestConvert<string[]>("abc")
                .Should().Equal(new[] { "abc" });
        }

        [Test]
        public void Convert_ListOfStringToArrayOfString()
        {
            TestConvert<string[]>(new List<string> { "abc", "def" })
                .Should().Equal("abc", "def");
        }

        [Test]
        public void Convert_ReadOnlyCollectionOfStringToArrayOfString()
        {
            TestConvert<string[]>(new ReadOnlyCollection<string>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");
        }

        [Test]
        public void Convert_ReadOnlyCollectionOfStringToIEnumerableOfString()
        {
            TestConvert<IEnumerable<string>>(new ReadOnlyCollection<string>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");
        }

        [Test]
        public void Convert_ReadOnlyCollectionOfObjectToIEnumerableOfString()
        {
            TestConvert<IEnumerable<string>>(new ReadOnlyCollection<object>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");
        }

        [Test]
        public void Convert_ReadOnlyCollectionOfObjectToReadOnlyCollectionOfString()
        {
            TestConvert<ReadOnlyCollection<string>>(new ReadOnlyCollection<object>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");
        }

        [Test]
        public void Convert_ReadOnlyCollectionOfObjectToQueueOfString()
        {
            TestConvert<Queue<string>>(new ReadOnlyCollection<object>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");
        }

        [Test]
        public void Convert_ReadOnlyCollectionOfIntToReadOnlyCollectionOfString()
        {
            TestConvert<ReadOnlyCollection<string>>(new ReadOnlyCollection<int>(new[] { 3, 2, 1 }))
                .Should().Equal("3", "2", "1");
        }

        [Test]
        public void Convert_ReadOnlyCollectionOfStringToReadOnlyCollectionOfInt()
        {
            TestConvert<ReadOnlyCollection<int>>(new ReadOnlyCollection<string>(new[] { "3", "2", "1" }))
                .Should().Equal(3, 2, 1);
        }

        [Test]
        public void Convert_ReadOnlyCollectionOfIntToListOfInt()
        {
            TestConvert<List<int>>(new ReadOnlyCollection<int>(new[] { 3, 2, 1 }))
                .Should().Equal(3, 2, 1);
        }

        [Test]
        public void Convert_ListOfIntToArrayOfString()
        {
            TestConvert<string[]>(new List<int> { 3, 2, 1 })
                .Should().Equal("3", "2", "1");
        }

        [Test]
        public void Convert_IEnumerableOfIntToArrayOfString()
        {
            TestConvert<string[]>(Enumerable.Range(1, 3))
                .Should().Equal("1", "2", "3");
        }

        private TDestination TestConvert<TDestination>(object sourceValue)
        {
            object result = sut.Convert(sourceValue, typeof(TDestination));

            return result.Should().BeAssignableTo<TDestination>().Subject;
        }
    }
}
