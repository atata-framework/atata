namespace Atata
{
    public class FindByValueAttribute : TermFindAttribute
    {
        public FindByValueAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByValueAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByValueAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByValueAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByAttributeStrategy("value");
        }
    }
}
