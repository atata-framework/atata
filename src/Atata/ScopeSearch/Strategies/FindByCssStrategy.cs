using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Atata
{
    public class FindByCssStrategy : IComponentScopeLocateStrategy
    {
        private readonly IComponentScopeLocateStrategy sequalStrategy = new FindFirstDescendantOrSelfStrategy();

        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            By by = By.CssSelector(string.Join(",", options.Terms));

            if (options.OuterXPath != null)
                by = new ByChained(By.XPath(options.OuterXPath + "*"), by);

            if (options.Index.HasValue)
            {
                var elements = scope.GetAll(by.With(searchOptions));
                if (elements.Count <= options.Index.Value)
                    throw ExceptionFactory.CreateForNoSuchElement(by: by, searchContext: scope);
                else
                    return new SequalComponentScopeLocateResult(elements[options.Index.Value], sequalStrategy);
            }
            else
            {
                return new SequalComponentScopeLocateResult(by, sequalStrategy);
            }
        }
    }
}
