namespace Atata
{
    public class FindByFieldSetStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath._("fieldset")[y => y._("legend")[z => z.TermsConditionOfContent]]).
                DescendantOrSelf.ComponentXPath;
        }
    }
}
