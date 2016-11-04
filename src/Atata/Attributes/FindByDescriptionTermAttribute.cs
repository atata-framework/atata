using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the description list term element. Finds the descendant control of the &lt;dd&gt; element in the scope of the &lt;dl&gt; element that has the &lt;dt&gt; element matching the specified term(s). Uses <c>Title</c> as the default term case.
    /// </summary>
    public class FindByDescriptionTermAttribute : TermFindAttribute
    {
        public FindByDescriptionTermAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByDescriptionTermAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public FindByDescriptionTermAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByDescriptionTermAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByDescriptionTermStrategy); }
        }
    }
}
