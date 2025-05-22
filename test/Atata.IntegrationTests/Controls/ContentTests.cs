namespace Atata.IntegrationTests.Controls;

public class ContentTests : WebDriverSessionTestSuite
{
    private ContentPage _page;

    private static TestCaseData[] ContentSourceTestCaseSource =>
    [
        new TestCaseData(ContentSource.Text).Returns("The quick brown fox jumps over the lazy dog."),
        new TestCaseData(ContentSource.TextContent).Returns("The quick brown fox jumps over the lazy dog."),
        new TestCaseData(ContentSource.InnerHtml).Returns("The quick <strong>brown</strong> fox <span><span>jumps</span> over</span> the lazy dog."),
        new TestCaseData(ContentSource.ChildTextNodes).Returns("The quick  fox  the lazy dog."),
        new TestCaseData(ContentSource.ChildTextNodesTrimmed).Returns("The quickfoxthe lazy dog."),
        new TestCaseData(ContentSource.ChildTextNodesTrimmedAndSpaceJoined).Returns("The quick fox the lazy dog."),
        new TestCaseData(ContentSource.FirstChildTextNode).Returns("The quick"),
        new TestCaseData(ContentSource.LastChildTextNode).Returns("the lazy dog.")
    ];

    protected override void OnSetUp() =>
        _page = Go.To<ContentPage>();

    [Test]
    public void Content_OfStringType() =>
        Go.To<ContentPage>()
            .VisibleDiv.Should.Be("Some text")
            .VisibleDiv.Content.Should.Be("Some text");

    [Test]
    public void Content_OfStringType_Invisible() =>
        Go.To<ContentPage>()
            .HiddenDiv.Should.Not.BeVisible()
            .HiddenDiv.Should.BeEmpty()
            .HiddenDiv.Content.Should.BeEmpty()
            .HiddenDivUsingTextContent.Should.Be("Some text")
            .HiddenDivUsingTextContent.Content.Should.Be("Some text")
            .HiddenDivUsingInnerHtml.Should.Be("Some <b>text</b>")
            .HiddenDivUsingInnerHtml.Content.Should.Be("Some <b>text</b>");

    [TestCaseSource(nameof(ContentSourceTestCaseSource))]
    public string GetContent(ContentSource contentSource) =>
        Go.To<ContentPage>()
            .ComplexTextControl.GetContent(contentSource);

    [TestCaseSource(nameof(ContentSourceTestCaseSource))]
    public string Text_UsingGetsContentFromSourceAttribute(ContentSource contentSource) =>
        Go.To<ContentPage>()
            .GetComplexText(contentSource).Value;

    [Test]
    public void Text()
    {
        VerifyEquals(_page.Text, "Some Text");
        VerifyEquals(_page.TextWithSpaces, "Some Text");
        VerifyEquals(_page.TextEmpty, string.Empty);
        _page.TextEmpty.Content.Should.Be(string.Empty);
    }

    [Test]
    public void Number()
    {
        VerifyEquals(_page.Number, 125.26m);
        VerifyEquals(_page.NumberZero, 0);
        VerifyEquals(_page.NumberNull, null);
        _page.NumberNull.Content.Should.Be(string.Empty);

        VerifyEquals(_page.NumberWithFormat, 59);
        VerifyDoesNotEqual(_page.NumberWithFormat, 55);
    }

    [Test]
    public void Currency()
    {
        VerifyEquals(_page.Currency, 125234.26m);
        VerifyDoesNotEqual(_page.Currency, 125234);
        VerifyEquals(_page.CurrencyFR, -123.456m);
    }

    [Test]
    public void Date()
    {
        VerifyEquals(_page.Date, new(2016, 5, 15));
        VerifyEquals(_page.DateNull, null);
        _page.DateNull.Content.Should.Be(string.Empty);

        VerifyEquals(_page.DateWithFormat, new(2016, 6, 15));
    }

    [Test]
    public void Time()
    {
        VerifyEquals(_page.Time, new(17, 15, 0));
        VerifyEquals(_page.TimeNull, null);
        _page.TimeNull.Content.Should.Be(string.Empty);

        VerifyEquals(_page.TimeOfDay, new(14, 45, 0));
    }

    [Test]
    [Platform(Exclude = Platforms.LinuxAndMacOS)] // 2025-05-22: Fails on pipelines for Linux and macOS.
    public void DateTime()
    {
        VerifyEquals(_page.DateTime, new(2016, 5, 15, 13, 45, 0));

        _page.DateTime.Should.EqualDate(new(2016, 5, 15))
            .DateTime.Should.BeGreater(new(2016, 5, 15))
            .DateTime.Should.BeLess(new(2016, 5, 16));
    }

    [Test]
    [Platform(Exclude = Platforms.LinuxAndMacOS)] // 2025-05-22: Fails on pipelines for Linux and macOS.
    public void DateTime_WithStandardFormat() =>
        VerifyEquals(_page.DateTimeWithFormat, new(2009, 6, 15, 13, 45, 0));

    [Test]
    public void DateTime_WithCustomFormat()
    {
        var sut = _page.DateTime;
        sut.Metadata.Push(new FormatAttribute("M/d/yyyy h:mm tt"));
        VerifyEquals(sut, new(2016, 5, 15, 13, 45, 0));
    }
}
