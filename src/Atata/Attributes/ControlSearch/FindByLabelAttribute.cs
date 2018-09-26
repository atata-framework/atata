using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the label element.
    /// Finds the &lt;label&gt; element by the specified term(s), then finds the bound control (for example, by label's "for" attribute referencing the element of the control by id).
    /// Uses <c>Title</c> as the default term case.
    /// </summary>
    public class FindByLabelAttribute : TermFindAttribute
    {
        public FindByLabelAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByLabelAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public FindByLabelAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByLabelAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByLabelStrategy); }
        }
    }
}
