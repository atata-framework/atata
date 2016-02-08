namespace Atata
{
    public class FindByContentOrValueAttribute : TermFindAttribute
    {
        public FindByContentOrValueAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByContentOrValueAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByContentOrValueAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByContentOrValueAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByContentOrValueStrategy();
        }
    }
}
