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

        [TestCase(Atata.TermFormat.TitleWithColon, "150text ", ExpectedResult = "150 Text:")]

        [TestCase(Atata.TermFormat.Sentence, "SimpleText", ExpectedResult = "Simple text")]
        [TestCase(Atata.TermFormat.Sentence, "Simple125Text", ExpectedResult = "Simple 125 text")]
        [TestCase(Atata.TermFormat.Sentence, " someUIText", ExpectedResult = "Some UI text")]
        [TestCase(Atata.TermFormat.Sentence, "T5Y", ExpectedResult = "T 5 y")]
        [TestCase(Atata.TermFormat.Sentence, "HTML5", ExpectedResult = "HTML 5")]

        [TestCase(Atata.TermFormat.SentenceLower, "Simple125Text", ExpectedResult = "simple 125 text")]

        [TestCase(Atata.TermFormat.Lower, "SimpleText", ExpectedResult = "simple text")]
        [TestCase(Atata.TermFormat.Lower, "HTML5", ExpectedResult = "html 5")]

        [TestCase(Atata.TermFormat.LowerMerged, "SimpleText", ExpectedResult = "simpletext")]

        [TestCase(Atata.TermFormat.Upper, "SimpleText", ExpectedResult = "SIMPLE TEXT")]
        [TestCase(Atata.TermFormat.Upper, "html5", ExpectedResult = "HTML 5")]

        [TestCase(Atata.TermFormat.UpperMerged, "html5", ExpectedResult = "HTML5")]

        [TestCase(Atata.TermFormat.Camel, "SimpleText", ExpectedResult = "simpleText")]
        [TestCase(Atata.TermFormat.Camel, "HTML5text", ExpectedResult = "html5Text")]
        [TestCase(Atata.TermFormat.Camel, "I 5W", ExpectedResult = "i5W")]

        [TestCase(Atata.TermFormat.Pascal, "SimpleText", ExpectedResult = "SimpleText")]
        [TestCase(Atata.TermFormat.Pascal, "html5Text", ExpectedResult = "Html5Text")]

        [TestCase(Atata.TermFormat.Kebab, "SimpleText", ExpectedResult = "simple-text")]
        [TestCase(Atata.TermFormat.Kebab, "HTML5text", ExpectedResult = "html-5-text")]
        [TestCase(Atata.TermFormat.Kebab, "5Text", ExpectedResult = "5-text")]
        [TestCase(Atata.TermFormat.Kebab, "Row6", ExpectedResult = "row-6")]

        [TestCase(Atata.TermFormat.XKebab, "SimpleText", ExpectedResult = "x-simple-text")]
        [TestCase(Atata.TermFormat.XKebab, "HTML5text", ExpectedResult = "x-html-5-text")]
        [TestCase(Atata.TermFormat.XKebab, "5Text", ExpectedResult = "x-5-text")]
        [TestCase(Atata.TermFormat.XKebab, "Row6", ExpectedResult = "x-row-6")]

        [TestCase(Atata.TermFormat.HyphenKebab, "HTML5text", ExpectedResult = "html‐5‐text")]

        [TestCase(Atata.TermFormat.Snake, "SimpleText", ExpectedResult = "simple_text")]
        [TestCase(Atata.TermFormat.Snake, "HTML5text", ExpectedResult = "html_5_text")]
        [TestCase(Atata.TermFormat.Snake, "5Text-5", ExpectedResult = "5_text_5")]
        [TestCase(Atata.TermFormat.Snake, "Row-6", ExpectedResult = "row_6")]

        [TestCase(Atata.TermFormat.PascalKebab, "html5Text", ExpectedResult = "Html-5-Text")]
        [TestCase(Atata.TermFormat.PascalHyphenKebab, "html5Text", ExpectedResult = "Html‐5‐Text")]
        public string TermFormat(TermFormat format, string value)
        {
            return format.ApplyTo(value);
        }
    }
}
