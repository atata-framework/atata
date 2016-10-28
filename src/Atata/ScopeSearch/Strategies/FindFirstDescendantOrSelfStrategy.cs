namespace Atata
{
    public class FindFirstDescendantOrSelfStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.DescendantOrSelf.ComponentXPath;
        }
    }
}
