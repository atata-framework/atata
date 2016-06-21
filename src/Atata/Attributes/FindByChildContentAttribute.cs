namespace Atata
{
    public class FindByChildContentAttribute : TermFindAttribute
    {
        public FindByChildContentAttribute(TermMatch match)
            : base(match)
        {
        }

        public FindByChildContentAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }

        public FindByChildContentAttribute(string value, TermMatch match)
            : base(value, match)
        {
        }

        public FindByChildContentAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public int ChildIndex { get; set; }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByChildContentStrategy(ChildIndex);
        }
    }
}
