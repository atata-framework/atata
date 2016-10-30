namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found by XPath. Finds the descendant or self control in the scope of the element found by the specified XPath.
    /// </summary>
    public class FindByXPathAttribute : FindAttribute, ITermFindAttribute
    {
        public FindByXPathAttribute(params string[] values)
        {
            Values = values;
        }

        public string[] Values { get; private set; }

        public string[] GetTerms(UIComponentMetadata metadata)
        {
            return Values;
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByXPathStrategy();
        }
    }
}
