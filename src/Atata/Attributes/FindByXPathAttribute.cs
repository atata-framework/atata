namespace Atata
{
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
