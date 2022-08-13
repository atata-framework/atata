namespace Atata
{
    public class FindItemByValueAttribute : TermFindItemAttribute
    {
        public FindItemByValueAttribute()
        {
        }

        public FindItemByValueAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindItemByValueAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindItemByValueAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public override IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata) =>
            new FindItemByValueStrategy();
    }
}
