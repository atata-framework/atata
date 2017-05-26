namespace Atata
{
    public class FindFirstDescendantStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.OuterXPath.ComponentXPath;
        }
    }
}
