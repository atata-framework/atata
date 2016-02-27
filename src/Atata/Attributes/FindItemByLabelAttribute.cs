namespace Atata
{
    public class FindItemByLabelAttribute : FindItemAttribute
    {
        public FindItemByLabelAttribute(TermMatch match = TermMatch.Equals)
        {
            Match = match;
        }

        public new TermMatch Match { get; private set; }

        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByLabelStrategy(Match);
        }
    }
}
