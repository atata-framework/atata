namespace Atata
{
    public class SelectByValueAttribute : SelectByAttribute
    {
        public SelectByValueAttribute(TermMatch match)
            : this(TermFormat.Inherit, match)
        {
        }

        public SelectByValueAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
