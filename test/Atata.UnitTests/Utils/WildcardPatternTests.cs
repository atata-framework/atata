namespace Atata.UnitTests.Utils;

public static class WildcardPatternTests
{
    public sealed class IsMatch
    {
        [TestCase("", "", ExpectedResult = true)]
        [TestCase("word", "", ExpectedResult = false)]
        [TestCase("", "word", ExpectedResult = false)]
        [TestCase("word", "*", ExpectedResult = true)]
        [TestCase("word", "**", ExpectedResult = true)]
        [TestCase("word", "*****", ExpectedResult = true)]
        [TestCase("word", "", ExpectedResult = false)]
        [TestCase("word", "word", ExpectedResult = true)]
        [TestCase("word", "word*", ExpectedResult = true)]
        [TestCase("word", "w*d", ExpectedResult = true)]
        [TestCase("word", "w***d", ExpectedResult = true)]
        [TestCase("word", "wor*", ExpectedResult = true)]
        [TestCase("word", "*rd", ExpectedResult = true)]
        [TestCase("word", "w*r", ExpectedResult = false)]
        [TestCase("word*", "word*", ExpectedResult = true)]
        [TestCase("word", "w?r?", ExpectedResult = true)]
        [TestCase("word", "word?", ExpectedResult = false)]
        [TestCase("wo*d", "w**d", ExpectedResult = true)]
        [TestCase("word do", "wo*do", ExpectedResult = true)]
        [TestCase("word do word do done", "wo*do*done", ExpectedResult = true)]

        [TestCase("a*b?c", "a\\*b\\?c", ExpectedResult = true)]
        [TestCase("aXb?c", "a\\*b\\?c", ExpectedResult = false)]
        [TestCase("a*bXc", "a\\*b\\?c", ExpectedResult = false)]
        public bool With(string input, string pattern) =>
            WildcardPattern.IsMatch(input, pattern);

        [Test]
        public void WithNullInput()
        {
            Action action = () => WildcardPattern.IsMatch(null, "some");

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'input')");
        }

        [Test]
        public void WithNullPattern()
        {
            Action action = () => WildcardPattern.IsMatch("some", null);

            action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'pattern')");
        }
    }

    public sealed class IsMatch_WithStringComparison
    {
        [TestCase("word", "WORD", StringComparison.InvariantCulture, ExpectedResult = false)]
        [TestCase("word", "WORD", StringComparison.InvariantCultureIgnoreCase, ExpectedResult = true)]
        public bool With(string input, string pattern, StringComparison stringComparison) =>
            WildcardPattern.IsMatch(input, pattern, stringComparison);
    }
}
