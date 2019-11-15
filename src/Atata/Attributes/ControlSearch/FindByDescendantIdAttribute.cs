using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the id attribute of any control's descendant.
    /// Finds the control that has any descendant having id matching the specified term(s).
    /// Uses <see cref="TermCase.Kebab"/> as the default term case.
    /// </summary>
    public class FindByDescendantIdAttribute : TermFindAttribute
    {
        public FindByDescendantIdAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByDescendantIdAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public FindByDescendantIdAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByDescendantIdAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Kebab; }
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByDescendantAttributeStrategy); }
        }

        protected override IEnumerable<object> GetStrategyArguments()
        {
            yield return "id";
        }
    }
}
