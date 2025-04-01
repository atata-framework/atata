namespace Atata.UnitTests.Terms;

public sealed class TermCaseResolverTests
{
    [TestCase(TermCase.None, "SimpleText", ExpectedResult = "SimpleText")]
    [TestCase(TermCase.None, "IsHTML5Text?", ExpectedResult = "IsHTML5Text?")]

    [TestCase(TermCase.Title, "SimpleText", ExpectedResult = "Simple Text")]
    [TestCase(TermCase.Title, "HTMLText", ExpectedResult = "HTML Text")]
    [TestCase(TermCase.Title, "IsHTMLText?", ExpectedResult = "Is HTML Text")]
    [TestCase(TermCase.Title, "I_O-P", ExpectedResult = "I O P")]
    [TestCase(TermCase.Title, "SomeUIProp", ExpectedResult = "Some UI Prop")]
    [TestCase(TermCase.Title, "SomeIProp", ExpectedResult = "Some I Prop")]
    [TestCase(TermCase.Title, "HTML5", ExpectedResult = "HTML 5")]
    [TestCase(TermCase.Title, "T5Y", ExpectedResult = "T 5 Y")]
    [TestCase(TermCase.Title, "Ta57YuD", ExpectedResult = "Ta 57 Yu D")]
    [TestCase(TermCase.Title, "text", ExpectedResult = "Text")]
    [TestCase(TermCase.Title, "150text", ExpectedResult = "150 Text")]
    [TestCase(TermCase.Title, "*-+=#@&^a%()", ExpectedResult = "A")]
    [TestCase(TermCase.Title, "TheBackUpOfADataFromOrNotIs", ExpectedResult = "The Back Up of a Data From or Not Is")]

    [TestCase(TermCase.Capitalized, "SimpleText", ExpectedResult = "Simple Text")]
    [TestCase(TermCase.Capitalized, "HTMLText", ExpectedResult = "HTML Text")]
    [TestCase(TermCase.Capitalized, "IsHTMLText?", ExpectedResult = "Is HTML Text")]
    [TestCase(TermCase.Capitalized, "I_O-P", ExpectedResult = "I O P")]
    [TestCase(TermCase.Capitalized, "SomeUIProp", ExpectedResult = "Some UI Prop")]
    [TestCase(TermCase.Capitalized, "SomeIProp", ExpectedResult = "Some I Prop")]
    [TestCase(TermCase.Capitalized, "HTML5", ExpectedResult = "HTML 5")]
    [TestCase(TermCase.Capitalized, "T5Y", ExpectedResult = "T 5 Y")]
    [TestCase(TermCase.Capitalized, "Ta57YuD", ExpectedResult = "Ta 57 Yu D")]
    [TestCase(TermCase.Capitalized, "text", ExpectedResult = "Text")]
    [TestCase(TermCase.Capitalized, "150text", ExpectedResult = "150 Text")]
    [TestCase(TermCase.Capitalized, "*-+=#@&^a%()", ExpectedResult = "A")]
    [TestCase(TermCase.Capitalized, "TheBackUpOfADataFromOrNotIs", ExpectedResult = "The Back Up Of A Data From Or Not Is")]

    [TestCase(TermCase.Sentence, "SimpleText", ExpectedResult = "Simple text")]
    [TestCase(TermCase.Sentence, "Simple125Text", ExpectedResult = "Simple 125 text")]
    [TestCase(TermCase.Sentence, " someUIText", ExpectedResult = "Some UI text")]
    [TestCase(TermCase.Sentence, "T5Y", ExpectedResult = "T 5 y")]
    [TestCase(TermCase.Sentence, "HTML5", ExpectedResult = "HTML 5")]

    [TestCase(TermCase.MidSentence, "Simple125Text", ExpectedResult = "simple 125 text")]

    [TestCase(TermCase.Lower, "SimpleText", ExpectedResult = "simple text")]
    [TestCase(TermCase.Lower, "HTML5", ExpectedResult = "html 5")]

    [TestCase(TermCase.LowerMerged, "SimpleText", ExpectedResult = "simpletext")]

    [TestCase(TermCase.Upper, "SimpleText", ExpectedResult = "SIMPLE TEXT")]
    [TestCase(TermCase.Upper, "html5", ExpectedResult = "HTML 5")]

    [TestCase(TermCase.UpperMerged, "html5", ExpectedResult = "HTML5")]

    [TestCase(TermCase.Camel, "SimpleText", ExpectedResult = "simpleText")]
    [TestCase(TermCase.Camel, "HTML5text", ExpectedResult = "html5Text")]
    [TestCase(TermCase.Camel, "I 5W", ExpectedResult = "i5W")]

    [TestCase(TermCase.Pascal, "SimpleText", ExpectedResult = "SimpleText")]
    [TestCase(TermCase.Pascal, "html5Text", ExpectedResult = "Html5Text")]

    [TestCase(TermCase.Kebab, "SimpleText", ExpectedResult = "simple-text")]
    [TestCase(TermCase.Kebab, "HTML5text", ExpectedResult = "html-5-text")]
    [TestCase(TermCase.Kebab, "5Text", ExpectedResult = "5-text")]
    [TestCase(TermCase.Kebab, "Row6", ExpectedResult = "row-6")]

    [TestCase(TermCase.HyphenKebab, "HTML5text", ExpectedResult = "html‐5‐text")]

    [TestCase(TermCase.Snake, "SimpleText", ExpectedResult = "simple_text")]
    [TestCase(TermCase.Snake, "HTML5text", ExpectedResult = "html_5_text")]
    [TestCase(TermCase.Snake, "5Text-5", ExpectedResult = "5_text_5")]
    [TestCase(TermCase.Snake, "Row-6", ExpectedResult = "row_6")]

    [TestCase(TermCase.PascalKebab, "html5Text", ExpectedResult = "Html-5-Text")]
    [TestCase(TermCase.PascalHyphenKebab, "html5Text", ExpectedResult = "Html‐5‐Text")]
    public string ApplyCase(TermCase termCase, string value) =>
        TermCaseResolver.ApplyCase(value, termCase);
}
