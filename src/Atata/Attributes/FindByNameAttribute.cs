namespace Atata
{
    public class FindByNameAttribute : TermFindAttribute
    {
        public FindByNameAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByNameAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByNameAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByNameAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Kebab; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByNameStrategy();
        }
    }
}
