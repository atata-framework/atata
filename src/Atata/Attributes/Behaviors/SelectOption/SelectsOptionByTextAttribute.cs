namespace Atata;

/// <summary>
/// Represents the behavior for option selection of <see cref="Select{TValue, TOwner}"/> control using option text.
/// </summary>
public class SelectsOptionByTextAttribute : SelectOptionBehaviorAttribute
{
    public SelectsOptionByTextAttribute()
    {
    }

    public SelectsOptionByTextAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public SelectsOptionByTextAttribute(TermMatch match)
        : base(match)
    {
    }

    public SelectsOptionByTextAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public override string FormatOptionXPathCondition(string value) =>
        Match.CreateXPathCondition(value);

    public override string GetOptionRawValue(IWebElement optionElement) =>
        optionElement.Text;
}
