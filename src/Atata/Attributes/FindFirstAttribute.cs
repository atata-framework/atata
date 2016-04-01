namespace Atata
{
    public class FindFirstAttribute : FindAttribute
    {
        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new XPathComponentScopeLocateStrategy();
        }
    }
}
