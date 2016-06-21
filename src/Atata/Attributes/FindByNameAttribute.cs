namespace Atata
{
    public class FindByNameAttribute : TermFindAttribute
    {
        public FindByNameAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByNameAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByNameAttribute(TermMatch match, params string[] values)
            : base(match, values)
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
