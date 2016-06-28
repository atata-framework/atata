namespace Atata
{
    public class SelectByTextAttribute : SelectByAttribute
    {
        public SelectByTextAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public SelectByTextAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }
    }
}
