namespace Atata
{
    public class FindItemByValueAttribute : TermSettingsAttribute, IFindItemAttribute
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

        public IItemElementFindStrategy CreateStrategy(UIComponent component, UIComponentMetadata metadata)
        {
            return new FindItemByValueStrategy();
        }
    }
}
