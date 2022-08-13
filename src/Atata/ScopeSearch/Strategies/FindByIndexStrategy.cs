namespace Atata
{
    public class FindByIndexStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
            builder.WrapWithIndex(options.Index.Value, x => x.OuterXPath.ComponentXPath);
    }
}
