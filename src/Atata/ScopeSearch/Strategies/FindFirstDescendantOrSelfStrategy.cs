namespace Atata
{
    public class FindFirstDescendantOrSelfStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
            builder.DescendantOrSelf.ComponentXPath;
    }
}
