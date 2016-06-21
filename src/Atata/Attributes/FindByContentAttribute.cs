namespace Atata
{
    public class FindByContentAttribute : TermFindAttribute
    {
        public FindByContentAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByContentAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByContentAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByContentAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByContentStrategy();
        }
    }
}
