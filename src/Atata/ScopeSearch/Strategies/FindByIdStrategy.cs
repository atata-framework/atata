namespace Atata
{
    public class FindByIdStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.Descendant.Any[y => y.TermsConditionOf("id")]).
                DescendantOrSelf.ComponentXPath;
        }
    }
}
