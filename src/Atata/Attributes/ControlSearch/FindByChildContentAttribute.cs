using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the child content text.
    /// Finds the control having the child with the specified content.
    /// Uses <see cref="TermCase.Title"/> as the default term case.
    /// </summary>
    public class FindByChildContentAttribute : TermFindAttribute
    {
        public FindByChildContentAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public FindByChildContentAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public FindByChildContentAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public FindByChildContentAttribute(params string[] values)
            : base(values)
        {
        }

        protected override TermCase DefaultCase => TermCase.Title;

        /// <summary>
        /// Gets or sets the index of the child element.
        /// The default value is <c>0</c>.
        /// </summary>
        public int ChildIndex { get; set; }

        protected override Type DefaultStrategy => typeof(FindByChildContentStrategy);

        protected override IEnumerable<object> GetStrategyArguments()
        {
            yield return ChildIndex;
        }
    }
}
