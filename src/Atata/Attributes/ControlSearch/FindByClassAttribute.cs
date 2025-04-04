namespace Atata;

/// <summary>
/// Specifies that a control should be found by class attribute.
/// Finds the descendant or self control in the scope of the element having the specified class.
/// Uses <see cref="TermCase.Kebab"/> as the default term case.
/// </summary>
public class FindByClassAttribute : TermFindAttribute
{
    public FindByClassAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByClassAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByClassAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByClassAttribute(params string[] values)
        : base(values)
    {
    }

    protected override TermCase DefaultCase => TermCase.Kebab;

    protected override Type DefaultStrategy => typeof(FindByClassStrategy);
}
