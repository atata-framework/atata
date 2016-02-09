namespace Atata
{
    public class FindByFieldsetAttribute : TermFindAttribute
    {
        public FindByFieldsetAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByFieldsetAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByFieldsetAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByFieldsetAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByFieldsetStrategy();
        }
    }
}
