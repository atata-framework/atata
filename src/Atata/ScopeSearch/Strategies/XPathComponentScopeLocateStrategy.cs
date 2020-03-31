using OpenQA.Selenium;

namespace Atata
{
    public abstract class XPathComponentScopeLocateStrategy : IComponentScopeFindStrategy
    {
        public ComponentScopeLocateResult Find(ISearchContext scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            ComponentScopeXPathBuilder builder = new ComponentScopeXPathBuilder(options);

            string xPath = Build(builder, options);

            return new XPathComponentScopeFindResult(xPath, scope, searchOptions);
        }

        protected abstract string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options);
    }
}
