using OpenQA.Selenium;

namespace Atata
{
    public class FindByCssStrategy : IComponentScopeLocateStrategy
    {
        private readonly XPathComponentScopeLocateStrategy sequalStrategy =
            new XPathComponentScopeLocateStrategy(XPathComponentScopeLocateStrategy.XPathPrefixKind.DescendantOrSelf, false);

        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            By by = By.CssSelector(string.Join(",", options.Terms));

            if (options.Index.HasValue)
            {
                var elements = scope.GetAll(by.With(searchOptions));
                if (elements.Count <= options.Index.Value)
                    throw ExceptionsFactory.CreateForNoSuchElement(by: by);
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
