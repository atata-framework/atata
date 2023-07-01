namespace Atata;

/// <summary>
/// Specifies that a control should be found by id attribute.
/// Finds the descendant or self control in the scope of the element having the specified id.
/// Uses <see cref="TermCase.Kebab"/> as the default term case.
/// </summary>
public class FindByIdAttribute : TermFindAttribute
{
    public FindByIdAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByIdAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByIdAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByIdAttribute(params string[] values)
        : base(values)
    {
    }

    /// <summary>
    /// Gets the default term case.
    /// The default value is <see cref="TermCase.Kebab"/>.
    /// </summary>
    protected override TermCase DefaultCase => TermCase.Kebab;

    protected override Type DefaultStrategy => typeof(FindByIdStrategy);
}
