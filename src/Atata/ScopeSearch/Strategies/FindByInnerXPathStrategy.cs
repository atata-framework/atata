namespace Atata
{
    public class FindByInnerXPathStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.WrapWithIndex(x => x.Descendant.ComponentXPath.Where(y => y.JoinOr(options.Terms)));
        }
    }
}
