namespace Atata;

/// <summary>
/// Specifies that a control should be found by the <c>alt</c> attribute.
/// Finds the control that has the <c>alt</c> attribute matching the specified term(s).
/// Uses <see cref="TermCase.Sentence"/> as the default term case.
/// </summary>
public class FindByAltAttribute : TermFindAttribute
{
    public FindByAltAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByAltAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByAltAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByAltAttribute(params string[] values)
        : base(values)
    {
    }

    protected override TermCase DefaultCase => TermCase.Sentence;

    protected override Type DefaultStrategy => typeof(FindByAttributeStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return "alt";
    }
}
