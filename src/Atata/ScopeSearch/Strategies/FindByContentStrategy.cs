namespace Atata
{
    public class FindByContentStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.TermsConditionOfContent]);
        }
    }
}
