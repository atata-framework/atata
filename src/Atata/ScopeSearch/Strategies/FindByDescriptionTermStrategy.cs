namespace Atata
{
    public class FindByDescriptionTermStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath._("dl/dt")[y => y.TermsConditionOfContent]).
                FollowingSibling._("dd").DescendantOrSelf.ComponentXPath;
        }
    }
}
