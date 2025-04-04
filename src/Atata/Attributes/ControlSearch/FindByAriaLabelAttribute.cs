#nullable enable

namespace Atata;

/// <summary>
/// Specifies that a control should be found by the <c>aria-label</c> attribute.
/// Finds the control that has the <c>aria-label</c> attribute matching the specified term(s).
/// Uses <see cref="TermCase.Sentence"/> as the default term case.
/// </summary>
public class FindByAriaLabelAttribute : TermFindAttribute
{
    public FindByAriaLabelAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByAriaLabelAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByAriaLabelAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByAriaLabelAttribute(params string[] values)
        : base(values)
    {
    }

    protected override TermCase DefaultCase => TermCase.Sentence;

    protected override Type DefaultStrategy => typeof(FindByAttributeStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return "aria-label";
    }
}
