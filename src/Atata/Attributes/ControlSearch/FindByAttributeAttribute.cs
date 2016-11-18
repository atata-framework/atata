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
            : base(termCase)
        {
            AttributeName = attributeName.CheckNotNullOrWhitespace(nameof(attributeName));
        }

        public FindByAttributeAttribute(string attributeName, TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
            AttributeName = attributeName.CheckNotNullOrWhitespace(nameof(attributeName));
        }

        public FindByAttributeAttribute(string attributeName, TermMatch match, params string[] values)
            : base(match, values)
        {
            AttributeName = attributeName.CheckNotNullOrWhitespace(nameof(attributeName));
        }

        public FindByAttributeAttribute(string attributeName, params string[] values)
            : base(values)
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
