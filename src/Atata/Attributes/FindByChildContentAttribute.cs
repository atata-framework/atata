namespace Atata
{
    public class FindByChildContentAttribute : TermFindAttribute
    {
        public FindByChildContentAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByChildContentAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
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

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public int ChildIndex { get; set; }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByChildContentStrategy(ChildIndex);
        }
    }
}
