namespace Atata
{
    public class FindByIndexAttribute : FindAttribute
    {
        public FindByIndexAttribute()
        {
        }

        public FindByIndexAttribute(int index)
        {
            Index = index;
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new XPathComponentScopeLocateStrategy();
        }
    }
}
