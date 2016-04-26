using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class TermFormatTest
    {
        [TestCase("SimpleText", ExpectedResult = "Simple Text")]
        [TestCase("HTMLText", ExpectedResult = "HTML Text")]
        [TestCase("IsHTMLText?", ExpectedResult = "Is HTML Text")]
        [TestCase("I_O-P", ExpectedResult = "I O P")]
        [TestCase("SomeUIProp", ExpectedResult = "Some UI Prop")]
        [TestCase("SomeIProp", ExpectedResult = "Some I Prop")]
        [TestCase("HTML5", ExpectedResult = "HTML 5")]
        [TestCase("T5Y", ExpectedResult = "T 5 Y")]
        [TestCase("Ta57YuD", ExpectedResult = "Ta 57 Yu D")]
        [TestCase("text", ExpectedResult = "Text")]
        [TestCase("150text", ExpectedResult = "150 Text")]
        [TestCase("*-+=#@&^a%()", ExpectedResult = "A")]
        public string TermFormat_Title(string value)
        {
            return TermFormat.Title.ApplyTo(value);
        }
    }
}
