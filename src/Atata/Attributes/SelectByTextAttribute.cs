namespace Atata
{
    public class SelectByTextAttribute : SelectByAttribute
    {
        public SelectByTextAttribute(TermMatch match)
            : this(TermFormat.Inherit, match)
        {
        }

        public SelectByTextAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
