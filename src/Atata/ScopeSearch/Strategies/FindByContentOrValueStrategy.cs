namespace Atata
{
    public class FindByContentOrValueStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
            builder.WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.TermsConditionOfContent.Or.TermsConditionOf("value")]);
    }
}
