namespace Atata
{
    public class FindItemByLabelAttribute : TermFindItemAttribute
    {
        public FindItemByLabelAttribute(TermFormat format)
            : base(format: format)
        {
        }

        public FindItemByLabelAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByLabelStrategy();
        }
    }
}
