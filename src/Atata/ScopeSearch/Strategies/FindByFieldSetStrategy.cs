namespace Atata
{
    public class FindByFieldSetStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.Descendant._("fieldset").Where(y => y._("legend").Where(z => z.TermsConditionOfContent))).
                DescendantOrSelf.ComponentXPath;
        }
    }
}
