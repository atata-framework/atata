namespace Atata.UnitTests;

public static class StringExtensionsTests
{
    public static class TrimStart
    {
        [Test]
        public static void WithNullValue() =>
            Assert.Throws<ArgumentNullException>(() =>
                (null as string).TrimStart("abc"));

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
}
