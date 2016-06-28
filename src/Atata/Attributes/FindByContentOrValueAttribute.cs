namespace Atata
{
    public class FindByContentOrValueAttribute : TermFindAttribute
    {
        public FindByContentOrValueAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByContentOrValueAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
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

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByContentOrValueStrategy();
        }
    }
}
