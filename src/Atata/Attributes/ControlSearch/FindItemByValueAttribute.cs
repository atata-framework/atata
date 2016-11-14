namespace Atata
{
    public class FindItemByValueAttribute : TermFindItemAttribute
    {
        public FindItemByValueAttribute(TermCase termCase)
            : base(termCase: termCase)
        {
        }

        public FindItemByValueAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByValueStrategy();
        }
    }
}
