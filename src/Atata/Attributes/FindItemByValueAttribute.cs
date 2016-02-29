namespace Atata
{
    public class FindItemByValueAttribute : FindItemAttribute
    {
        public FindItemByValueAttribute(TermMatch match = TermMatch.Equals)
        {
            Match = match;
        }

        public new TermMatch Match { get; private set; }

        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByValueStrategy(Match);
        }
    }
}
