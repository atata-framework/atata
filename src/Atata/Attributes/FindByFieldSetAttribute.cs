namespace Atata
{
    public class FindByFieldSetAttribute : TermFindAttribute
    {
        public FindByFieldSetAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByFieldSetAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByFieldSetAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByFieldSetAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByFieldSetStrategy();
        }
    }
}
