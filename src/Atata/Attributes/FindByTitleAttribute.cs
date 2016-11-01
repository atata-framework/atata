namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the title attribute. Finds the control that has the title attribute matching the specified term(s). Uses <c>Title</c> as the default term case.
    /// </summary>
    public class FindByTitleAttribute : TermFindAttribute
    {
        public FindByTitleAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByTitleAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByTitleAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByTitleAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByAttributeStrategy("title");
        }
    }
}
