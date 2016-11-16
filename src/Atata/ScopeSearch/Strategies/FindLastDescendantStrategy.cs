namespace Atata
{
    public class FindLastDescendantStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.Wrap(x => x.Descendant.ComponentXPath)["last()"];
        }
    }
}
