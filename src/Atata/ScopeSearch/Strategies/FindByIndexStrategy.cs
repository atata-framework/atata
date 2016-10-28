namespace Atata
{
    public class FindByIndexStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.WrapWithIndex(options.Index.Value, x => x.Descendant.ComponentXPath);
        }
    }
}
