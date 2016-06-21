namespace Atata
{
    public class FindByChildContentAttribute : TermFindAttribute
    {
        public FindByChildContentAttribute(TermFormat format)
            : base(format)
        {
        }

        public FindByChildContentAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public FindByChildContentAttribute(TermMatch match, params string[] values)
            : base(match, values)
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
