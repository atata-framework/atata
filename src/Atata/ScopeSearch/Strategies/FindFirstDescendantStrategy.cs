namespace Atata
{
    public class FindFirstDescendantStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
            builder.OuterXPath.ComponentXPath;
    }
}
