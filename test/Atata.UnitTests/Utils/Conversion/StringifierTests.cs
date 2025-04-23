namespace Atata.UnitTests.Utils.Conversion;

public static class StringifierTests
{
    public new sealed class ToString
    {
        [Test]
        public void WithMultilineString() =>
            ResultOf(
                """
                a
                b
                c
                """)
                .Should.Be("""
                    "a
                    b
                    c"
                    """);

        [Test]
        public void WithArrayOfMultilineStrings() =>
            ResultOf(
                new[]
                {
                    """
                    a
                    b
                    """,
                    """
                    c
                      d
                    """,
                    "e"
                })
                .Should.Be("""
                    [
                      "a
                      b",
                      "c
                        d",
                      "e"
                    ]
                    """);

        [Test]
        public void WithArrayOfIntegers() =>
            ResultOf(
                new[] { 1, 2, 3 })
                .Should.Be("[1, 2, 3]");

        [Test]
        public void WithArrayOfBooleans() =>
            ResultOf(
                new[] { true, false })
                .Should.Be("[true, false]");

        [Test]
        public void WithArrayOfRecords() =>
            ResultOf(
                new[]
                {
                    new SomeRecord(1, "a"),
                    new SomeRecord(2, "b")
                })
                .Should.Be("[{ SomeRecord { Id = 1, Name = a } }, { SomeRecord { Id = 2, Name = b } }]");

        [Test]
        public void WithArrayOfRecordsContainingMultilineStringAndNull() =>
            ResultOf(
                new[]
                {
                    new SomeRecord(1, "a"),
                    new SomeRecord(2, "b" + Environment.NewLine + "c"),
                    null
                })
                .Should.Be("""
                    [
                      { SomeRecord { Id = 1, Name = a } },
                      {
                        SomeRecord { Id = 2, Name = b
                        c }
                      },
                      null
                    ]
                    """);

        private static Subject<string> ResultOf(object value) =>
            Subject.ResultOf(() => Stringifier.ToString(value));

        public sealed record class SomeRecord(int Id, string Name);
    }

    public sealed class ToStringInShortForm
    {
        [TestCase(typeof(Uri), ExpectedResult = "Uri")]
        [TestCase(typeof(bool), ExpectedResult = "bool")]
        [TestCase(typeof(List<Uri>), ExpectedResult = "List<Uri>")]
        [TestCase(typeof(List<int?>), ExpectedResult = "List<int?>")]
        [TestCase(typeof(Dictionary<int, string>), ExpectedResult = "Dictionary<int, string>")]
        [TestCase(typeof(Dictionary<int, string>.KeyCollection.Enumerator), ExpectedResult = "Dictionary<int, string>.KeyCollection.Enumerator")]
        [TestCase(typeof(Top<int, string>.Middle<float>.ILow<Uri, List<int>.Enumerator>), ExpectedResult = "StringifierTests.Top<int, string>.Middle<float>.ILow<Uri, List<int>.Enumerator>")]
        public string With(Type type) =>
            Stringifier.ToStringInShortForm(type);
    }

#pragma warning disable S2326 // Unused type parameters should be removed
    public class Top<T1, T2>
    {
        public class Middle<T3>
        {
            public interface ILow<T4, T5>;
        }
    }
#pragma warning restore S2326 // Unused type parameters should be removed
}
