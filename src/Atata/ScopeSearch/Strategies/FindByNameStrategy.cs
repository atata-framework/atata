namespace Atata
{
    public class FindByNameStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.Any[y => y.TermsConditionOf("name")]).
                DescendantOrSelf.ComponentXPath;
        }
    }
}
