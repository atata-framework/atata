namespace Atata
{
    public class SelectByValueAttribute : SelectByAttribute
    {
        public SelectByValueAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public SelectByValueAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }
    }
}
