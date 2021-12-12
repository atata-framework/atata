namespace Atata
{
    public class FindFirstDescendantOrSelfStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.DescendantOrSelf.ComponentXPath;
        }
    }
}
