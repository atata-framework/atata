namespace Atata
{
    public class FindItemByValueAttribute : TermFindItemAttribute
    {
        public FindItemByValueAttribute(TermMatch match)
            : this(TermFormat.Title, match)
        {
        }

        public FindItemByValueAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByValueStrategy(this);
        }
    }
}
