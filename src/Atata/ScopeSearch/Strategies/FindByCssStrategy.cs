using OpenQA.Selenium;

namespace Atata
{
    public class FindByCssStrategy : IComponentScopeFindStrategy
    {
        private readonly IComponentScopeFindStrategy sequalStrategy = new FindFirstDescendantOrSelfStrategy();

        public ComponentScopeLocateResult Find(ISearchContext scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
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
                    {
                        return new MissingComponentScopeFindResult();
                    }
                    else
                    {
                        throw ExceptionFactory.CreateForNoSuchElement(
                            new SearchFailureData
                            {
                                ElementName = $"{(options.Index.Value + 1).Ordinalize()} matching selector",
                                By = by,
                                SearchOptions = searchOptions,
                                SearchContext = scope
                            });
                    }
                }
                else
                {
                    return new SubsequentComponentScopeFindResult(elements[options.Index.Value], sequalStrategy);
                }
            }
            else
            {
                return new SubsequentComponentScopeFindResult(by, sequalStrategy);
            }
        }
    }
}
