using OpenQA.Selenium;

namespace Atata
{
    public class FindByIndexStrategy : XPathComponentScopeLocateStrategy
    {
        public override ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            if (options.ElementXPath == "*" && !options.HasIndex)
                return new XPathComponentScopeLocateResult("self::*", scope, searchOptions);
            else
                return base.Find(scope, options, searchOptions);
        }
    }
}
