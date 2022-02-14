using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by CSS selector.
    /// Finds the descendant or self control in the scope of the element found by the specified CSS selector.
    /// </summary>
    public class FindByCssAttribute : FindAttribute, ITermFindAttribute
    {
        public FindByCssAttribute(params string[] values)
        {
            values.CheckNotNullOrEmpty(nameof(values));

            Values = values;
        }

        /// <summary>
        /// Gets the CSS selector values.
        /// </summary>
        public string[] Values { get; }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByCssStrategy); }
        }

        public string[] GetTerms(UIComponentMetadata metadata)
        {
            return Values;
        }

        public override string BuildComponentName(UIComponentMetadata metadata) =>
            BuildComponentNameWithArgument(string.Join("/", Values));
    }
}
