namespace Atata
{
    public class FindFirstDescendantStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.OuterXPath.ComponentXPath;
        }
    }
}
