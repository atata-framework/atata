namespace Atata
{
    public class FindByIndexAttribute : FindAttribute
    {
        public FindByIndexAttribute(int index)
        {
            Index = index;
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new XPathComponentScopeLocateStrategy(XPathComponentScopeLocateStrategy.XPathPrefixKind.DescendantOrSelf);
        }
    }
}
