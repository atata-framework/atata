namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the placeholder attribute. Finds the control that has the placeholder attribute matching the value. Uses <c>Title</c> as the default term case.
    /// </summary>
    public class FindByPlaceholderAttribute : TermFindAttribute
    {
        public FindByPlaceholderAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByPlaceholderAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByPlaceholderAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByPlaceholderAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByAttributeStrategy("placeholder");
        }
    }
}
