namespace Atata
{
    public abstract class TermMatchFindAttribute : TermFindAttribute, ITermMatchFindAttribute
    {
        protected TermMatchFindAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(format)
        {
            Match = match;
        }

        protected TermMatchFindAttribute(params string[] values)
            : base(values)
        {
        }

        public new TermMatch Match { get; set; }

        public TermMatch GetTermMatch(UIComponentMetadata metadata)
        {
            return Match != TermMatch.Inherit ? Match : GetTremMatchFromMetadata(metadata);
        }

        protected abstract TermMatch GetTremMatchFromMetadata(UIComponentMetadata metadata);
    }
}
