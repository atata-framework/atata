namespace Atata
{
    public class FindByContentOrValueStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.Descendant.ComponentXPath.Where(y => y.TermsConditionOfContent.Or.TermsConditionOf("value")));
        }
    }
}
