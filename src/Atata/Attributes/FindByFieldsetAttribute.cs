namespace Atata
{
    public class FindByFieldsetAttribute : TermFindAttribute
    {
        public FindByFieldsetAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByFieldsetAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByFieldsetAttribute(TermMatch match, params string[] values)
            : base(match, values)
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
