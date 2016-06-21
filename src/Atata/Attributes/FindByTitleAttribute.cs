namespace Atata
{
    public class FindByTitleAttribute : TermFindAttribute
    {
        public FindByTitleAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByTitleAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByTitleAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByTitleAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByAttributeStrategy("title");
        }
    }
}
