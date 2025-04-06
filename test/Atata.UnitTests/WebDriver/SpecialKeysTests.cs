using OpenQA.Selenium;

namespace Atata.UnitTests.WebDriver;

public sealed class SpecialKeysTests
{
    public sealed class Replace
    {
        [Test]
        public void WithNull() =>
            Subject.Invoking(() => SpecialKeys.Replace(null!))
                .Should.ThrowExactly<ArgumentNullException>();

        [TestCase("", ExpectedResult = "")]
        [TestCase("a", ExpectedResult = "a")]
        [TestCase("some text", ExpectedResult = "some text")]
        public string With(string keys) =>
            SpecialKeys.Replace(keys);

        [Test]
        public void WithSpecialKeys()
        {
            var keys = SpecialKeys.Replace($"some{Keys.Enter}text{Keys.Control}");

            Subject.ResultOf(() => SpecialKeys.Replace(keys))
                .Should.Be("some<Enter>text<Control>");
        }
    }
}
