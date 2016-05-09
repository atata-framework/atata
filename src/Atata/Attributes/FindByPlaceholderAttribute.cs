namespace Atata
{
    public class FindByPlaceholderAttribute : TermFindAttribute
    {
        public FindByPlaceholderAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByPlaceholderAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByPlaceholderAttribute(string value, TermMatch match)
            : base(value, match)
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
