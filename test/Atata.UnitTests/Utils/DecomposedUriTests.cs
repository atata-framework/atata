namespace Atata.UnitTests.Utils;

public static class DecomposedUriTests
{
    public sealed class Ctor
    {
        [Test]
        public void WithNullUrl() =>
            new DecomposedUri(null)
                .Should().BeEquivalentTo(new DecomposedUri());

        [Test]
        public void WithEmptyUrl() =>
            new DecomposedUri(string.Empty)
                .Should().BeEquivalentTo(new DecomposedUri());

        [Test]
        public void WithUrlThatHasOnlyFragment() =>
            new DecomposedUri("#frg")
                .Should().BeEquivalentTo(new DecomposedUri
                {
                    Fragment = "frg"
                });

        [Test]
        public void WithUrlThatHasOnlyQuery() =>
            new DecomposedUri("?a=1")
                .Should().BeEquivalentTo(new DecomposedUri
                {
                    Query = "a=1"
                });

        [Test]
        public void WithUrlThatHasOnlyPath() =>
            new DecomposedUri("some/path")
                .Should().BeEquivalentTo(new DecomposedUri
                {
                    FullPath = "some/path"
                });

        [Test]
        public void WithUrlThatIsAbsoluteAndHasAllParts() =>
            new DecomposedUri("https://example.org/some/path?a=1&b=2#frg")
                .Should().BeEquivalentTo(new DecomposedUri
                {
                    FullPath = "https://example.org/some/path",
                    Query = "a=1&b=2",
                    Fragment = "frg"
                });

        [Test]
        public void WithUrlThatIsRelativeAndHasAllParts() =>
            new DecomposedUri("/some/path?a=1&b=2#frg")
                .Should().BeEquivalentTo(new DecomposedUri
                {
                    FullPath = "/some/path",
                    Query = "a=1&b=2",
                    Fragment = "frg"
                });
    }

    public new sealed class ToString
    {
        [TestCase("https://example.org/some/path?a=1&b=2#frg", ExpectedResult = "https://example.org/some/path?a=1&b=2#frg")]
        [TestCase("/some/path", ExpectedResult = "/some/path")]
        [TestCase("some/path", ExpectedResult = "some/path")]
        [TestCase("?a=1&b=2#frg", ExpectedResult = "?a=1&b=2#frg")]
        [TestCase("&a=1", ExpectedResult = "?a=1")]
        [TestCase(";a=1", ExpectedResult = "?a=1")]
        [TestCase("#frg", ExpectedResult = "#frg")]
        public string With(string uri) =>
            new DecomposedUri(uri).ToString();
    }

    public sealed class Merge
    {
        [TestCase("/some/path?a=1", "/other/path", ExpectedResult = "/other/path")]
        [TestCase("/some/path?a=1", "?c", ExpectedResult = "/some/path?c")]
        [TestCase("/some/path?a=1&b=2#frg", "?c=3", ExpectedResult = "/some/path?c=3")]
        [TestCase("/some/path?a=1&b=2#frg", "&c=3", ExpectedResult = "/some/path?a=1&b=2&c=3")]
        [TestCase("/some/path?a=1;b=2", ";c=3", ExpectedResult = "/some/path?a=1;b=2;c=3")]
        [TestCase("/some/path?a=1", "?", ExpectedResult = "/some/path")]
        [TestCase("/some/path#frg", "#frg2", ExpectedResult = "/some/path#frg2")]
        [TestCase("/some/path?a=1#frg", "#frg2", ExpectedResult = "/some/path?a=1#frg2")]
        [TestCase("/some/path?a=1#frg", "?b=2#frg2", ExpectedResult = "/some/path?b=2#frg2")]
        [TestCase("/some/path?a=1#frg", "&b=2#frg2", ExpectedResult = "/some/path?a=1&b=2#frg2")]
        [TestCase("/some/path?a=1#frg", "#", ExpectedResult = "/some/path?a=1")]
        public string With(string uri1, string uri2) =>
            DecomposedUri.Merge(uri1, uri2);
    }
}
