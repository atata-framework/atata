namespace Atata
{
    public class FindByContentOrValueAttribute : TermFindAttribute
    {
        public FindByContentOrValueAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByContentOrValueAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByContentOrValueAttribute(TermMatch match, params string[] values)
            : base(match, values)
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
