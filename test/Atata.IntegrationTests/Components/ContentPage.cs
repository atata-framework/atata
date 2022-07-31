namespace Atata.IntegrationTests;

using _ = ContentPage;

[Url("content")]
[VerifyTitle]
[FindByDescriptionTerm(TargetType = typeof(Content<,>))]
public class ContentPage : Page<_>
{
    public Text<_> Text { get; private set; }

    public Text<_> TextWithSpaces { get; private set; }

    [FindById]
    public Text<_> TextNull { get; private set; }

    public Number<_> Number { get; private set; }

    public Number<_> NumberZero { get; private set; }

    [FindById]
    public Number<_> NumberNull { get; private set; }

    [Term("Number")]
    public Text<_> NumberAsText { get; private set; }

    [Format("count: {0}")]
    public Number<_> NumberWithFormat { get; private set; }

    public Currency<_> Currency { get; private set; }

    [Format("C3")]
    [Culture("fr-FR")]
    public Currency<_> CurrencyFR { get; private set; }

    public Date<_> Date { get; private set; }

    [FindById]
    public Date<_> DateNull { get; private set; }

    [Format("{0:dd, MMMM yyyy} year")]
    public Date<_> DateWithFormat { get; private set; }

    public Time<_> Time { get; private set; }

    [FindById]
    public Time<_> TimeNull { get; private set; }

    [Format("h:mm tt")]
    public Time<_> TimeOfDay { get; private set; }

    [Term(TermCase.Pascal)]
    public DateTime<_> DateTime { get; private set; }

    [Term("DateTime with Format")]
    [Format("f")]
    public DateTime<_> DateTimeWithFormat { get; private set; }

    [FindByDescriptionTerm("Complex Text")]
    public Control<_> ComplexTextControl { get; private set; }

    [FindById]
    [FindSettings(Visibility = Visibility.Any)]
    public Content<string, _> HiddenDiv { get; private set; }

    [FindById("hidden-div", Visibility = Visibility.Visible)]
    public Content<string, _> HiddenDivWithVisibleVisibility { get; private set; }

    [FindById("hidden-div", Visibility = Visibility.Hidden)]
    [GetsContentFromSource(ContentSource.TextContent)]
    public Content<string, _> HiddenDivUsingTextContent { get; private set; }

    [FindById("hidden-div", Visibility = Visibility.Any)]
    [GetsContentFromSource(ContentSource.InnerHtml)]
    public Content<string, _> HiddenDivUsingInnerHtml { get; private set; }

    [FindById]
    public Content<string, _> VisibleDiv { get; private set; }

    public Text<_> GetComplexText(ContentSource contentSource)
    {
        return Find<Text<_>>(
            $"Complex Text Using {contentSource} Source",
            new TermAttribute("Complex Text"),
            new GetsContentFromSourceAttribute(contentSource));
    }
}
