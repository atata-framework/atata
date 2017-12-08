using OpenQA.Selenium;

namespace Atata
{
    public class FindByCssStrategy : IComponentScopeLocateStrategy
    {
        private readonly IComponentScopeLocateStrategy sequalStrategy = new FindFirstDescendantOrSelfStrategy();

        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            By by = By.CssSelector(string.Join(",", options.Terms));

            if (options.OuterXPath != null)
                by = By.XPath(options.OuterXPath + "*").Then(by);

            if (options.Index.HasValue)
            {
                var elements = scope.GetAll(by.With(searchOptions));
                if (elements.Count <= options.Index.Value)
                {
                    if (searchOptions.IsSafely)
                        return new MissingComponentScopeLocateResult();
                    else
                        throw ExceptionFactory.CreateForNoSuchElement(by: by, searchContext: scope);
                }
                else
                {
                    return new SequalComponentScopeLocateResult(elements[options.Index.Value], sequalStrategy);
                }
            }
            else
            {
                return new SequalComponentScopeLocateResult(by, sequalStrategy);
            }
        }
    }
}
