namespace Atata
{
    public class FindByDescriptionTermAttribute : TermFindAttribute
    {
        public FindByDescriptionTermAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByDescriptionTermAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByDescriptionTermAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByDescriptionTermAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByDescriptionTermStrategy();
        }
    }
}
