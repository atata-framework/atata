using OpenQA.Selenium;

namespace Atata
{
    public abstract class XPathComponentScopeLocateStrategy : IComponentScopeLocateStrategy
    {
        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            ComponentScopeXPathBuilder builder = new ComponentScopeXPathBuilder(options);

            string xPath = Build(builder, options);

            return new XPathComponentScopeLocateResult(xPath, scope, searchOptions);
        }

        protected abstract string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options);
    }
}
