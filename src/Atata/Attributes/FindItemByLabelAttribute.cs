namespace Atata
{
    public class FindItemByLabelAttribute : TermFindItemAttribute
    {
        public FindItemByLabelAttribute(TermCase termCase)
            : base(termCase: termCase)
        {
        }

        public FindItemByLabelAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public override IItemElementFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindItemByLabelStrategy();
        }
    }
}
