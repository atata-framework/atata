namespace Atata.UnitTests;

public static class StringExtensionsTests
{
    public static class TrimStart
    {
        [Test]
        public static void WithNullValue() =>
            Assert.Throws<ArgumentNullException>(() =>
                ((null as string)!).TrimStart("abc"));

        [TestCase("abcdef", null, ExpectedResult = "abcdef")]
        [TestCase("abcdef", "", ExpectedResult = "abcdef")]
        [TestCase("abcdef", "abc", ExpectedResult = "def")]
        [TestCase("abcdef", "Abc", ExpectedResult = "abcdef")]
        [TestCase("abcdef", "bcd", ExpectedResult = "abcdef")]
        [TestCase("abcdef", "abcdef", ExpectedResult = "")]
        [TestCase("abcdef", "abcdefg", ExpectedResult = "abcdef")]
        public static string With(string value, string trimString) =>
            value.TrimStart(trimString);
    }

    public static class Sanitize_WithReplacement
    {
        [Test]
        public static void WithNullInvalidChars() =>
            Assert.Throws<ArgumentNullException>(() =>
                "some".Sanitize(null!, '_'));

        [TestCase("abcdef", new[] { 'c', 'f' }, 'X', ExpectedResult = "abXdeX")]
        [TestCase("abcdef", new char[] { }, 'X', ExpectedResult = "abcdef")]
        [TestCase("Abcdef", new[] { 'a', 'F' }, 'X', ExpectedResult = "Abcdef")]
        [TestCase("aaa", new[] { 'a', 'a' }, 'X', ExpectedResult = "XXX")]
        [TestCase("", new[] { 'a', }, 'X', ExpectedResult = "")]
        public static string With(string value, char[] invalidChars, char replaceWith) =>
            value.Sanitize(invalidChars, replaceWith);
    }

    public static class Sanitize_WithoutReplacement
    {
        [Test]
        public static void WithNullInvalidChars() =>
            Assert.Throws<ArgumentNullException>(() =>
                "some".Sanitize(null!));

        [TestCase("abcdef", new[] { 'c', 'f' }, ExpectedResult = "abde")]
        [TestCase("abcdef", new char[] { }, ExpectedResult = "abcdef")]
        [TestCase("Abcdef", new[] { 'a', 'F' }, ExpectedResult = "Abcdef")]
        [TestCase("aaa", new[] { 'a', 'a' }, ExpectedResult = "")]
        [TestCase("", new[] { 'a', }, ExpectedResult = "")]
        public static string With(string value, char[] invalidChars) =>
            value.Sanitize(invalidChars);
    }
}
