using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by the specified attribute. Finds the control that has the specified attribute matching the specified term(s). Uses <c>Title</c> as the default term case.
    /// </summary>
    public class FindByAttributeAttribute : TermFindAttribute
    {
        public FindByAttributeAttribute(string attributeName, TermCase termCase)
            : this(attributeName, null, termCase: termCase)
        {
        }

        public FindByAttributeAttribute(string attributeName, TermMatch match, TermCase termCase = TermCase.Inherit)
            : this(attributeName, null, match, termCase)
        {
        }

        public FindByAttributeAttribute(string attributeName, TermMatch match, params string[] values)
            : this(attributeName, values, match)
        {
        }

        public FindByAttributeAttribute(string attributeName, params string[] values)
            : this(attributeName, values, termCase: TermCase.Inherit)
        {
        }

        private FindByAttributeAttribute(string attributeName, string[] values = null, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(values, match, termCase)
        {
            AttributeName = attributeName.CheckNotNullOrWhitespace(nameof(attributeName));
        }

        public string AttributeName { get; private set; }

        protected override TermCase DefaultCase
        {
            get { return TermCase.Title; }
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByAttributeStrategy); }
        }

        protected override IEnumerable<object> GetStrategyArguments()
        {
            yield return AttributeName;
        }
    }
}
