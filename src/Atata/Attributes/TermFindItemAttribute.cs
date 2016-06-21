namespace Atata
{
    public abstract class TermFindItemAttribute : FindItemAttribute, ITermSettings
    {
        protected TermFindItemAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
        {
            Match = match;
            Format = format;
        }

        public new TermMatch Match { get; private set; }
        public TermFormat Format { get; private set; }
        public string StringFormat { get; set; }
    }
}
