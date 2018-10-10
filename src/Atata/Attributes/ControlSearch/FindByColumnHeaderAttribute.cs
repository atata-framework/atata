using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found within the table column (&lt;td&gt;) that has the header (&lt;th&gt;) matching the specified term(s).
    /// Uses <c>Title</c> as the default term case.
    /// </summary>
    public class FindByColumnHeaderAttribute : TermFindAttribute
    {
        public FindByColumnHeaderAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByColumnHeaderAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public FindByColumnHeaderAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByColumnHeaderAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByColumnHeaderStrategy); }
        }
    }
}
