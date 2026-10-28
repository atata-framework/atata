namespace Atata.UnitTests.Utils;

public class UriUtilsTests
{
    [TestCase("http://something.com", true)]
    [TestCase("https://something.com", true)]
    [TestCase("ftp://something.com", true)]
    [TestCase("custom://something.com", true)]
    [TestCase("http:/something.com", false)]
    [TestCase("//something", false)]
    [TestCase("/something", false)]
    [TestCase("something", false)]
    [TestCase(null, false)]
    public void TryCreateAbsoluteUrl(string url, bool isAbsolute)
    {
        var isActuallyAbsolute = UriUtils.TryCreateAbsoluteUrl(url, out Uri? result);
        isActuallyAbsolute.Should().Be(isAbsolute);

        if (isAbsolute)
            result!.AbsoluteUri.Should().StartWith(url);
        else
            result.Should().BeNull();
    }

    [TestCase("HTTPS://something.com")]
    [TestCase("HtTpS://something.com")]
    public void TryCreateAbsoluteUrl_WithCaseInsensitiveScheme(string url)
    {
        UriUtils.TryCreateAbsoluteUrl(url, out Uri? result).Should().BeTrue();
        result!.Scheme.Should().Be("https");
    }

    [TestCase("http://some.com", "path", ExpectedResult = "http://some.com/path")]
    [TestCase("http://some.com/", "path", ExpectedResult = "http://some.com/path")]
    [TestCase("http://some.com", "/path", ExpectedResult = "http://some.com/path")]
    [TestCase("http://some.com/", "/path", ExpectedResult = "http://some.com/path")]
    [TestCase("http://some.com", null, ExpectedResult = "http://some.com/")]
    public string Concat(string baseUri, string relativeUri) =>
        UriUtils.Concat(baseUri, relativeUri).AbsoluteUri;

    [Test]
    public void Concat_WithNullBaseUrl() =>
        Assert.Throws<ArgumentNullException>(() =>
            UriUtils.Concat(null!, "/path"));
}
