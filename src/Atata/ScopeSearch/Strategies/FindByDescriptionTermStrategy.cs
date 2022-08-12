namespace Atata
{
    public class FindByDescriptionTermStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath._("dl/dt")[y => y.TermsConditionOfContent]).
                FollowingSibling._("dd").DescendantOrSelf.ComponentXPath;
        }
    }
}
