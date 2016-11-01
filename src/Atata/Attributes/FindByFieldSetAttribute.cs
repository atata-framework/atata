namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the parent fieldset element. Finds the descendant control in the scope of the &lt;fieldset&gt; element that has the &lt;legend&gt; element matching the specified term(s). Uses <c>Title</c> as the default term case.
    /// </summary>
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
