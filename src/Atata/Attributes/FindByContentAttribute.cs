namespace Atata
{
    public class FindByContentAttribute : TermFindAttribute
    {
        public FindByContentAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByContentAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
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

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByContentStrategy();
        }
    }
}
