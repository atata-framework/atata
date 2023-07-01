namespace Atata;

/// <summary>
/// Specifies that a control should be found by the <c>aria-labelledby</c> attribute.
/// Finds the control that has the <c>aria-labelledby</c> attribute matching the specified term(s).
/// Uses <see cref="TermCase.Kebab"/> as the default term case.
/// </summary>
public class FindByAriaLabelledByAttribute : TermFindAttribute
{
    public FindByAriaLabelledByAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByAriaLabelledByAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByAriaLabelledByAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByAriaLabelledByAttribute(params string[] values)
        : base(values)
    {
    }

    protected override TermCase DefaultCase => TermCase.Kebab;

    protected override Type DefaultStrategy => typeof(FindByAttributeStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return "aria-labelledby";
    }
}
