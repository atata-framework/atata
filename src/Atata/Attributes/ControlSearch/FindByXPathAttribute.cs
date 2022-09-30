using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by XPath.
    /// Finds the descendant or self control in the scope of the element found by the specified XPath.
    /// </summary>
    public class FindByXPathAttribute : FindAttribute, ITermFindAttribute
    {
        public FindByXPathAttribute(params string[] values)
        {
            values.CheckNotNullOrEmpty(nameof(values));

            Values = values;
        }

        /// <summary>
        /// Gets the XPath values.
        /// </summary>
        public string[] Values { get; }

        protected override Type DefaultStrategy => typeof(FindByXPathStrategy);

        public string[] GetTerms(UIComponentMetadata metadata) => Values;

        public override string BuildComponentName(UIComponentMetadata metadata) =>
            BuildComponentNameWithArgument(string.Join(" or ", Values));
    }
}
