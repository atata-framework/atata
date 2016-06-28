namespace Atata
{
    public class FindByDescriptionTermAttribute : TermFindAttribute
    {
        public FindByDescriptionTermAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByDescriptionTermAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByDescriptionTermAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByDescriptionTermAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByDescriptionTermStrategy();
        }
    }
}
