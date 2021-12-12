namespace Atata
{
    public class FindByIdStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.Any[y => y.TermsConditionOf("id")]).
                DescendantOrSelf.ComponentXPath;
        }
    }
}
