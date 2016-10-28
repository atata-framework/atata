namespace Atata
{
    public class UseParentScopeStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.Self.Any;
        }
    }
}
