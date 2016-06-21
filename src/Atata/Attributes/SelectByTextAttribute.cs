namespace Atata
{
    public class SelectByTextAttribute : SelectByAttribute
    {
        public SelectByTextAttribute(TermFormat format)
            : base(format)
        {
        }

        public SelectByTextAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }
    }
}
