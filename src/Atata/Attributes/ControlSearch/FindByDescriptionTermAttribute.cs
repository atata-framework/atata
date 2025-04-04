#nullable enable

namespace Atata;

/// <summary>
/// Specifies that a control should be found by the description list term element.
/// Finds the descendant control of the <c>&lt;dd&gt;</c> element in the scope of the <c>&lt;dl&gt;</c> element that has the <c>&lt;dt&gt;</c> element matching the specified term(s).
/// Uses <see cref="TermCase.Title"/> as the default term case.
/// </summary>
public class FindByDescriptionTermAttribute : TermFindAttribute
{
    public FindByDescriptionTermAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByDescriptionTermAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByDescriptionTermAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByDescriptionTermAttribute(params string[] values)
        : base(values)
    {
    }

    protected override TermCase DefaultCase => TermCase.Title;

    protected override Type DefaultStrategy => typeof(FindByDescriptionTermStrategy);
}
