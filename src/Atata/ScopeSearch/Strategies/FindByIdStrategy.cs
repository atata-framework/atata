namespace Atata
{
    public class FindByIdStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.Any[y => y.TermsConditionOf("id")]).
                DescendantOrSelf.ComponentXPath;
        }
    }
}
