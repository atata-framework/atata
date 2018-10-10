using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the content text.
    /// Finds the control having the specified content.
    /// Uses <c>Title</c> as the default term case.
    /// </summary>
    public class FindByContentAttribute : TermFindAttribute
    {
        public FindByContentAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByContentAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public FindByContentAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByContentAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByContentStrategy); }
        }
    }
}
