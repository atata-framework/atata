using System.Collections.ObjectModel;

namespace Atata.UnitTests.Utils.Conversion;

public static class ObjectConverterTests
{
    [TestFixture]
    public class Convert
    {
        private ObjectConverter _sut;

        [SetUp]
        public void SetUp() =>
            _sut = new ObjectConverter();

        [Test]
        public void NullToString() =>
            _sut.Convert(null, typeof(string))
                .Should().BeNull();

        [Test]
        public void NullToArrayOfInt() =>
            _sut.Convert(null, typeof(int[]))
                .Should().BeNull();

        [Test]
        public void NullToInt_Throws() =>
            Assert.Throws<ConversionException>(() =>
                _sut.Convert(null, typeof(int)));

        [Test]
        public void StringToDecimal() =>
            TestConvert<decimal>("5.555")
                .Should().Be(5.555m);

        [Test]
        public void DecimalToString() =>
            TestConvert<string>(5.555m)
                .Should().Be("5.555");

        [Test]
        public void StringToBool() =>
            TestConvert<bool>("true")
                .Should().Be(true);

        [Test]
        public void StringToNullableBool() =>
            TestConvert<bool?>("true")
                .Should().Be(true);

        [Test]
        public void BoolToString() =>
            TestConvert<string>(true)
                .Should().Be("True");

        [Test]
        public void StringToEnum() =>
            TestConvert<TermCase>(nameof(TermCase.Kebab))
                .Should().Be(TermCase.Kebab);

        [TestCase("findByIdAttribute", ExpectedResult = typeof(FindByIdAttribute))]
        [TestCase("ordinaryPage", ExpectedResult = typeof(OrdinaryPage))]
        [TestCase("testPage", ExpectedResult = typeof(TestPage))]
        public Type StringToType(string value) =>
            TestConvert<Type>(value);

        [Test]
        public void StringToArrayOfString() =>
            TestConvert<string[]>("abc")
                .Should().Equal("abc");

        [Test]
        public void ListOfStringToArrayOfString() =>
            TestConvert<string[]>(new List<string> { "abc", "def" })
                .Should().Equal("abc", "def");

        [Test]
        public void ReadOnlyCollectionOfStringToArrayOfString() =>
            TestConvert<string[]>(new ReadOnlyCollection<string>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");

        [Test]
        public void ReadOnlyCollectionOfStringToIEnumerableOfString() =>
            TestConvert<IEnumerable<string>>(new ReadOnlyCollection<string>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");

        [Test]
        public void ReadOnlyCollectionOfObjectToIEnumerableOfString() =>
            TestConvert<IEnumerable<string>>(new ReadOnlyCollection<object>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");

        [Test]
        public void ReadOnlyCollectionOfObjectToReadOnlyCollectionOfString() =>
            TestConvert<ReadOnlyCollection<string>>(new ReadOnlyCollection<object>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");

        [Test]
        public void ReadOnlyCollectionOfObjectToQueueOfString() =>
            TestConvert<Queue<string>>(new ReadOnlyCollection<object>(new[] { "abc", "def" }))
                .Should().Equal("abc", "def");

        [Test]
        public void ReadOnlyCollectionOfIntToReadOnlyCollectionOfString() =>
            TestConvert<ReadOnlyCollection<string>>(new ReadOnlyCollection<int>(new[] { 3, 2, 1 }))
                .Should().Equal("3", "2", "1");

        [Test]
        public void ReadOnlyCollectionOfStringToReadOnlyCollectionOfInt() =>
            TestConvert<ReadOnlyCollection<int>>(new ReadOnlyCollection<string>(new[] { "3", "2", "1" }))
                .Should().Equal(3, 2, 1);

        [Test]
        public void ReadOnlyCollectionOfIntToListOfInt() =>
            TestConvert<List<int>>(new ReadOnlyCollection<int>(new[] { 3, 2, 1 }))
                .Should().Equal(3, 2, 1);

        [Test]
        public void ListOfIntToArrayOfString() =>
            TestConvert<string[]>(new List<int> { 3, 2, 1 })
                .Should().Equal("3", "2", "1");

        [Test]
        public void IEnumerableOfIntToArrayOfString() =>
            TestConvert<string[]>(Enumerable.Range(1, 3))
                .Should().Equal("1", "2", "3");

        private TDestination TestConvert<TDestination>(object sourceValue)
        {
            object result = _sut.Convert(sourceValue, typeof(TDestination));

            return result.Should().BeAssignableTo<TDestination>().Subject;
        }
    }
}
