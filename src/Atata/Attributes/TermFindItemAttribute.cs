namespace Atata
{
    public abstract class TermFindItemAttribute : FindItemAttribute, ITermSettings
    {
        protected TermFindItemAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
        {
            Match = match;
            Case = termCase;
        }

        public new TermMatch Match { get; private set; }
        public TermCase Case { get; private set; }
        public string StringFormat { get; set; }
    }
}
