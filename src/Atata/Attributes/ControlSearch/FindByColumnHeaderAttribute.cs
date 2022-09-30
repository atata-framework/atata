using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found within the table column (<c>&lt;td&gt;</c>) that has the header (<c>&lt;th&gt;</c>) matching the specified term(s).
    /// Uses <see cref="TermCase.Title"/> as the default term case.
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

        protected override TermCase DefaultCase => TermCase.Title;

        protected override Type DefaultStrategy => typeof(FindByColumnHeaderStrategy);
    }
}
