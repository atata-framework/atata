namespace Atata
{
    public class FindItemByValueAttribute : TermFindItemAttribute
    {
        public FindItemByValueAttribute(TermFormat format)
            : base(format: format)
        {
        }

        public FindItemByValueAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByValueStrategy();
        }
    }
}
