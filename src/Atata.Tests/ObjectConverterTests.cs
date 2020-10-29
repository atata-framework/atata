using System;
using System.Collections.Generic;
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
        public void Convert_ListOfIntToArrayOfString()
        {
            TestConvert<string[]>(new List<int> { 3, 2, 1 })
                .Should().Equal("3", "2", "1");
        }

        private TDestination TestConvert<TDestination>(object sourceValue)
        {
            object result = sut.Convert(sourceValue, typeof(TDestination));

            return result.Should().BeAssignableTo<TDestination>().Subject;
        }
    }
}
