using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the content text or value attribute.
    /// Finds the control that has the content or value attribute matching the specified term(s).
    /// Uses <see cref="TermCase.Title"/> as the default term case.
    /// </summary>
    public class FindByContentOrValueAttribute : TermFindAttribute
    {
        public FindByContentOrValueAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByContentOrValueAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public FindByContentOrValueAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByContentOrValueAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByContentOrValueStrategy); }
        }
    }
}
