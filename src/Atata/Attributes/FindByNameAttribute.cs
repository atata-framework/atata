namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by name attribute. Finds the descendant or self control in the scope of the element having the specified name. Uses <c>Kebab</c> as the default term case.
    /// </summary>
    public class FindByNameAttribute : TermFindAttribute
    {
        public FindByNameAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByNameAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByNameAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByNameAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Kebab; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByNameStrategy();
        }
    }
}
