namespace Atata
{
    public class FindByClassAttribute : TermFindAttribute
    {
        public FindByClassAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByClassAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByClassAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByClassAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Kebab; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByClassStrategy();
        }
    }
}
