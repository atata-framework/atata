#nullable enable

namespace Atata;

public class FindByCssStrategy : IComponentScopeFindStrategy
{
    private readonly IComponentScopeFindStrategy _sequalStrategy = new FindFirstDescendantOrSelfStrategy();

    public ComponentScopeFindResult Find(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
    {
        By by = By.CssSelector(string.Join(",", options.Terms));

        if (options.OuterXPath is not null)
            by = By.XPath(options.OuterXPath + "*").Then(by);

        if (options.Index.HasValue)
        {
            var elements = scope.GetAllWithLogging(
                options.Component.Log,
                by.With(searchOptions));

            if (elements.Count <= options.Index.Value)
            {
                if (searchOptions.IsSafely)
                {
                    return ComponentScopeFindResult.Missing;
                }
                else
                {
                    throw ElementExceptionFactory.CreateForNotFound(
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
                return new SubsequentComponentScopeFindResult(elements[options.Index.Value], _sequalStrategy);
            }
        }
        else
        {
            return new SubsequentComponentScopeFindResult(by, _sequalStrategy);
        }
    }
}
