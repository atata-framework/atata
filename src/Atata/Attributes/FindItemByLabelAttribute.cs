namespace Atata
{
    public class FindItemByLabelAttribute : TermFindItemAttribute
    {
        public FindItemByLabelAttribute(TermMatch match)
            : this(TermFormat.Title, match)
        {
        }

        public FindItemByLabelAttribute(TermFormat format = TermFormat.Title, TermMatch match = TermMatch.Equals)
            : base(format, match)
        {
        }

        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByLabelStrategy(this);
        }
    }
}
