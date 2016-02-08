namespace Atata
{
    public class FindByValueAttribute : TermFindAttribute
    {
        public FindByValueAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByValueAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByValueAttribute(string value, TermMatch match)
            : base(value, match)
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
