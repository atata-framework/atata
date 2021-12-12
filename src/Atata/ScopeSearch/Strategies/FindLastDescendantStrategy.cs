namespace Atata
{
    public class FindLastDescendantStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.Wrap(x => x.OuterXPath.ComponentXPath)["last()"];
        }
    }
}
