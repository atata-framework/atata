namespace Atata
{
    public class FindByContentAttribute : TermFindAttribute
    {
        public FindByContentAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByContentAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByContentAttribute(string value, TermMatch match)
            : base(value, match)
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
