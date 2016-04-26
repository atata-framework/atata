using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class TermFormatTest
    {
        [TestCase(Atata.TermFormat.None, "SimpleText", ExpectedResult = "SimpleText")]
        [TestCase(Atata.TermFormat.None, "IsHTML5Text?", ExpectedResult = "IsHTML5Text?")]

        [TestCase(Atata.TermFormat.Title, "SimpleText", ExpectedResult = "Simple Text")]
        [TestCase(Atata.TermFormat.Title, "HTMLText", ExpectedResult = "HTML Text")]
        [TestCase(Atata.TermFormat.Title, "IsHTMLText?", ExpectedResult = "Is HTML Text")]
        [TestCase(Atata.TermFormat.Title, "I_O-P", ExpectedResult = "I O P")]
        [TestCase(Atata.TermFormat.Title, "SomeUIProp", ExpectedResult = "Some UI Prop")]
        [TestCase(Atata.TermFormat.Title, "SomeIProp", ExpectedResult = "Some I Prop")]
        [TestCase(Atata.TermFormat.Title, "HTML5", ExpectedResult = "HTML 5")]
        [TestCase(Atata.TermFormat.Title, "T5Y", ExpectedResult = "T 5 Y")]
        [TestCase(Atata.TermFormat.Title, "Ta57YuD", ExpectedResult = "Ta 57 Yu D")]
        [TestCase(Atata.TermFormat.Title, "text", ExpectedResult = "Text")]
        [TestCase(Atata.TermFormat.Title, "150text", ExpectedResult = "150 Text")]
        [TestCase(Atata.TermFormat.Title, "*-+=#@&^a%()", ExpectedResult = "A")]

        [TestCase(Atata.TermFormat.Sentence, "SimpleText", ExpectedResult = "Simple text")]
        [TestCase(Atata.TermFormat.Sentence, "Simple125Text", ExpectedResult = "Simple 125 text")]
        [TestCase(Atata.TermFormat.Sentence, "SomeUIText", ExpectedResult = "Some UI text")]
        [TestCase(Atata.TermFormat.Sentence, "T5Y", ExpectedResult = "T 5 y")]

        [TestCase(Atata.TermFormat.LowerCase, "SimpleText", ExpectedResult = "simple text")]
        [TestCase(Atata.TermFormat.LowerCase, "HTML5", ExpectedResult = "html 5")]

        [TestCase(Atata.TermFormat.UpperCase, "SimpleText", ExpectedResult = "SIMPLE TEXT")]
        [TestCase(Atata.TermFormat.UpperCase, "html5", ExpectedResult = "HTML 5")]

        [TestCase(Atata.TermFormat.Camel, "SimpleText", ExpectedResult = "simpleText")]
        [TestCase(Atata.TermFormat.Camel, "HTML5text", ExpectedResult = "html5Text")]

        [TestCase(Atata.TermFormat.Pascal, "SimpleText", ExpectedResult = "SimpleText")]
        [TestCase(Atata.TermFormat.Pascal, "html5Text", ExpectedResult = "Html5Text")]

        [TestCase(Atata.TermFormat.Dashed, "SimpleText", ExpectedResult = "simple-text")]
        [TestCase(Atata.TermFormat.Dashed, "HTML5text", ExpectedResult = "html-5-text")]
        [TestCase(Atata.TermFormat.Dashed, "5Text", ExpectedResult = "5-text")]
        [TestCase(Atata.TermFormat.Dashed, "Row6", ExpectedResult = "row-6")]

        [TestCase(Atata.TermFormat.XDashed, "SimpleText", ExpectedResult = "x-simple-text")]
        [TestCase(Atata.TermFormat.XDashed, "HTML5text", ExpectedResult = "x-html-5-text")]
        [TestCase(Atata.TermFormat.XDashed, "5Text", ExpectedResult = "x-5-text")]
        [TestCase(Atata.TermFormat.XDashed, "Row6", ExpectedResult = "x-row-6")]

        [TestCase(Atata.TermFormat.Underscored, "SimpleText", ExpectedResult = "simple_text")]
        [TestCase(Atata.TermFormat.Underscored, "HTML5text", ExpectedResult = "html_5_text")]
        [TestCase(Atata.TermFormat.Underscored, "5Text-5", ExpectedResult = "5_text_5")]
        [TestCase(Atata.TermFormat.Underscored, "Row-6", ExpectedResult = "row_6")]
        public string TermFormat(TermFormat format, string value)
        {
            return format.ApplyTo(value);
        }
    }
}
