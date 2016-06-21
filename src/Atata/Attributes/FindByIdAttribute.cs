namespace Atata
{
    public class FindByIdAttribute : TermFindAttribute
    {
        public FindByIdAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByIdAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByIdAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByIdAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Kebab; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByIdStrategy();
        }
    }
}
