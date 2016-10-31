namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the value attribute. Finds the control that has the value attribute matching the value. Uses <c>Title</c> as the default term case.
    /// </summary>
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
