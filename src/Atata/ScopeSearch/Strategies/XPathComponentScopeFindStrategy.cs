namespace Atata;

public abstract class XPathComponentScopeFindStrategy : IComponentScopeFindStrategy
{
    public ComponentScopeFindResult Find(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
    {
        ComponentScopeXPathBuilder builder = new ComponentScopeXPathBuilder(options);

        string xPath = Build(builder, options);

        return new XPathComponentScopeFindResult(xPath, scope, searchOptions);
    }

    protected abstract string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options);
}
