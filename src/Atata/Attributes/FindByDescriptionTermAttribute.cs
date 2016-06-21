namespace Atata
{
    public class FindByDescriptionTermAttribute : TermFindAttribute
    {
        public FindByDescriptionTermAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByDescriptionTermAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByDescriptionTermAttribute(TermMatch match, params string[] values)
            : base(match, values)
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
