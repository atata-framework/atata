namespace Atata
{
    public class UseParentScopeAttribute : FindAttribute
    {
        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new XPathComponentScopeLocateStrategy(
                XPathComponentScopeLocateStrategy.XPathPrefixKind.DescendantOrSelf,
                XPathComponentScopeLocateStrategy.IndexUsage.AnyCase);
        }
    }
}
