namespace Atata
{
    public class FindByInnerXPathAttribute : FindAttribute, ITermFindAttribute
    {
        public FindByInnerXPathAttribute(params string[] values)
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
            return new FindByInnerXPathStrategy();
        }
    }
}
