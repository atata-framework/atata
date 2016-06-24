namespace Atata
{
    public class FindByFieldSetAttribute : TermFindAttribute
    {
        public FindByFieldSetAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByFieldSetAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByFieldSetAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByFieldSetAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByFieldSetStrategy();
        }
    }
}
