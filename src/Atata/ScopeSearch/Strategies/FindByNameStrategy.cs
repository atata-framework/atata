namespace Atata
{
    public class FindByNameStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.Any[y => y.TermsConditionOf("name")]).
                DescendantOrSelf.ComponentXPath;
        }
    }
}
