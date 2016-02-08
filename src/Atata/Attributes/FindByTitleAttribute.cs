namespace Atata
{
    public class FindByTitleAttribute : TermFindAttribute
    {
        public FindByTitleAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByTitleAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByTitleAttribute(string value, TermMatch match)
            : base(value, match)
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
