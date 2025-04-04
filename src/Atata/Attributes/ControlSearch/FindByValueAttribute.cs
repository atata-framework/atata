namespace Atata;

/// <summary>
/// Specifies that a control should be found by the value attribute.
/// Finds the control that has the value attribute matching the specified term(s).
/// Uses <see cref="TermCase.Title"/> as the default term case.
/// </summary>
public class FindByValueAttribute : TermFindAttribute
{
    public FindByValueAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByValueAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByValueAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByValueAttribute(params string[] values)
        : base(values)
    {
    }

    protected override TermCase DefaultCase => TermCase.Title;

    protected override Type DefaultStrategy => typeof(FindByAttributeStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return "value";
    }
}
