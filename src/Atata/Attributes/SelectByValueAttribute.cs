namespace Atata
{
    public class SelectByValueAttribute : SelectByAttribute
    {
        public SelectByValueAttribute(TermFormat format)
            : base(format)
        {
        }

        public SelectByValueAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }
    }
}
