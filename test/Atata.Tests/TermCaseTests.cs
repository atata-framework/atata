using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class TermCaseTests
    {
        [TestCase(Atata.TermCase.None, "SimpleText", ExpectedResult = "SimpleText")]
        [TestCase(Atata.TermCase.None, "IsHTML5Text?", ExpectedResult = "IsHTML5Text?")]

        [TestCase(Atata.TermCase.Title, "SimpleText", ExpectedResult = "Simple Text")]
        [TestCase(Atata.TermCase.Title, "HTMLText", ExpectedResult = "HTML Text")]
        [TestCase(Atata.TermCase.Title, "IsHTMLText?", ExpectedResult = "Is HTML Text")]
        [TestCase(Atata.TermCase.Title, "I_O-P", ExpectedResult = "I O P")]
        [TestCase(Atata.TermCase.Title, "SomeUIProp", ExpectedResult = "Some UI Prop")]
        [TestCase(Atata.TermCase.Title, "SomeIProp", ExpectedResult = "Some I Prop")]
        [TestCase(Atata.TermCase.Title, "HTML5", ExpectedResult = "HTML 5")]
        [TestCase(Atata.TermCase.Title, "T5Y", ExpectedResult = "T 5 Y")]
        [TestCase(Atata.TermCase.Title, "Ta57YuD", ExpectedResult = "Ta 57 Yu D")]
        [TestCase(Atata.TermCase.Title, "text", ExpectedResult = "Text")]
        [TestCase(Atata.TermCase.Title, "150text", ExpectedResult = "150 Text")]
        [TestCase(Atata.TermCase.Title, "*-+=#@&^a%()", ExpectedResult = "A")]
        [TestCase(Atata.TermCase.Title, "TheBackUpOfADataFromOrNotIs", ExpectedResult = "The Back Up of a Data from or Not Is")]

        [TestCase(Atata.TermCase.Capitalized, "SimpleText", ExpectedResult = "Simple Text")]
        [TestCase(Atata.TermCase.Capitalized, "HTMLText", ExpectedResult = "HTML Text")]
        [TestCase(Atata.TermCase.Capitalized, "IsHTMLText?", ExpectedResult = "Is HTML Text")]
        [TestCase(Atata.TermCase.Capitalized, "I_O-P", ExpectedResult = "I O P")]
        [TestCase(Atata.TermCase.Capitalized, "SomeUIProp", ExpectedResult = "Some UI Prop")]
        [TestCase(Atata.TermCase.Capitalized, "SomeIProp", ExpectedResult = "Some I Prop")]
        [TestCase(Atata.TermCase.Capitalized, "HTML5", ExpectedResult = "HTML 5")]
        [TestCase(Atata.TermCase.Capitalized, "T5Y", ExpectedResult = "T 5 Y")]
        [TestCase(Atata.TermCase.Capitalized, "Ta57YuD", ExpectedResult = "Ta 57 Yu D")]
        [TestCase(Atata.TermCase.Capitalized, "text", ExpectedResult = "Text")]
        [TestCase(Atata.TermCase.Capitalized, "150text", ExpectedResult = "150 Text")]
        [TestCase(Atata.TermCase.Capitalized, "*-+=#@&^a%()", ExpectedResult = "A")]
        [TestCase(Atata.TermCase.Capitalized, "TheBackUpOfADataFromOrNotIs", ExpectedResult = "The Back Up Of A Data From Or Not Is")]

        [TestCase(Atata.TermCase.Sentence, "SimpleText", ExpectedResult = "Simple text")]
        [TestCase(Atata.TermCase.Sentence, "Simple125Text", ExpectedResult = "Simple 125 text")]
        [TestCase(Atata.TermCase.Sentence, " someUIText", ExpectedResult = "Some UI text")]
        [TestCase(Atata.TermCase.Sentence, "T5Y", ExpectedResult = "T 5 y")]
        [TestCase(Atata.TermCase.Sentence, "HTML5", ExpectedResult = "HTML 5")]

        [TestCase(Atata.TermCase.MidSentence, "Simple125Text", ExpectedResult = "simple 125 text")]

        [TestCase(Atata.TermCase.Lower, "SimpleText", ExpectedResult = "simple text")]
        [TestCase(Atata.TermCase.Lower, "HTML5", ExpectedResult = "html 5")]

        [TestCase(Atata.TermCase.LowerMerged, "SimpleText", ExpectedResult = "simpletext")]

        [TestCase(Atata.TermCase.Upper, "SimpleText", ExpectedResult = "SIMPLE TEXT")]
        [TestCase(Atata.TermCase.Upper, "html5", ExpectedResult = "HTML 5")]

        [TestCase(Atata.TermCase.UpperMerged, "html5", ExpectedResult = "HTML5")]

        [TestCase(Atata.TermCase.Camel, "SimpleText", ExpectedResult = "simpleText")]
        [TestCase(Atata.TermCase.Camel, "HTML5text", ExpectedResult = "html5Text")]
        [TestCase(Atata.TermCase.Camel, "I 5W", ExpectedResult = "i5W")]

        [TestCase(Atata.TermCase.Pascal, "SimpleText", ExpectedResult = "SimpleText")]
        [TestCase(Atata.TermCase.Pascal, "html5Text", ExpectedResult = "Html5Text")]

        [TestCase(Atata.TermCase.Kebab, "SimpleText", ExpectedResult = "simple-text")]
        [TestCase(Atata.TermCase.Kebab, "HTML5text", ExpectedResult = "html-5-text")]
        [TestCase(Atata.TermCase.Kebab, "5Text", ExpectedResult = "5-text")]
        [TestCase(Atata.TermCase.Kebab, "Row6", ExpectedResult = "row-6")]

        [TestCase(Atata.TermCase.HyphenKebab, "HTML5text", ExpectedResult = "html‐5‐text")]

        [TestCase(Atata.TermCase.Snake, "SimpleText", ExpectedResult = "simple_text")]
        [TestCase(Atata.TermCase.Snake, "HTML5text", ExpectedResult = "html_5_text")]
        [TestCase(Atata.TermCase.Snake, "5Text-5", ExpectedResult = "5_text_5")]
        [TestCase(Atata.TermCase.Snake, "Row-6", ExpectedResult = "row_6")]

        [TestCase(Atata.TermCase.PascalKebab, "html5Text", ExpectedResult = "Html-5-Text")]
        [TestCase(Atata.TermCase.PascalHyphenKebab, "html5Text", ExpectedResult = "Html‐5‐Text")]
        public string TermCase(TermCase termCase, string value)
        {
            return termCase.ApplyTo(value);
        }
    }
}
