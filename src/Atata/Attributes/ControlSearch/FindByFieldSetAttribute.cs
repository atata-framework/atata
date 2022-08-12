using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the parent fieldset element.
    /// Finds the descendant control in the scope of the <c>&lt;fieldset&gt;</c> element that has the <c>&lt;legend&gt;</c> element matching the specified term(s).
    /// Uses <see cref="TermCase.Title"/> as the default term case.
    /// </summary>
    public class FindByFieldSetAttribute : TermFindAttribute
    {
        public FindByFieldSetAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByFieldSetAttribute(TermMatch match, TermCase termCase)
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

        protected override TermCase DefaultCase => TermCase.Title;

        protected override Type DefaultStrategy => typeof(FindByFieldSetStrategy);
    }
}
