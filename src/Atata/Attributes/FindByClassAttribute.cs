namespace Atata
{
    public class FindByClassAttribute : TermFindAttribute
    {
        public FindByClassAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByClassAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByClassAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByClassAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Kebab; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByClassStrategy();
        }
    }
}
