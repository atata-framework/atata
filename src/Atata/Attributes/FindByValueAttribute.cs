namespace Atata
{
    public class FindByValueAttribute : TermFindAttribute
    {
        public FindByValueAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByValueAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByValueAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByValueAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByAttributeStrategy("value");
        }
    }
}
