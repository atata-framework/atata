namespace Atata
{
    public class FindByIdAttribute : TermFindAttribute
    {
        public FindByIdAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByIdAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByIdAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByIdAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Kebab; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByIdStrategy();
        }
    }
}
