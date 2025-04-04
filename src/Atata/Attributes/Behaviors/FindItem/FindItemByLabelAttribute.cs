namespace Atata;

public class FindItemByLabelAttribute : TermFindItemAttribute
{
    public FindItemByLabelAttribute()
    {
    }

    public FindItemByLabelAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindItemByLabelAttribute(TermMatch match)
        : base(match)
    {
    }

    public FindItemByLabelAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public override IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata) =>
        new FindItemByLabelStrategy(component);
}
