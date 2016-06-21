namespace Atata
{
    public class FindByPlaceholderAttribute : TermFindAttribute
    {
        public FindByPlaceholderAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByPlaceholderAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByPlaceholderAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByPlaceholderAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByAttributeStrategy("placeholder");
        }
    }
}
