namespace Atata;

public class StrategyScopeLocator : IScopeLocator
{
    private readonly IStrategyScopeLocatorExecutionDataCollector _executionDataCollector;

    private readonly IStrategyScopeLocatorExecutor _executor;

    public StrategyScopeLocator(
        IStrategyScopeLocatorExecutionDataCollector executionDataCollector,
        IStrategyScopeLocatorExecutor executor)
    {
        _executionDataCollector = executionDataCollector;
        _executor = executor;
    }

    public IWebElement? GetElement(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        var executionData = _executionDataCollector.Get(searchOptions);
        XPathComponentScopeFindResult[] xPathResults = _executor.Execute(executionData);

        if (xPathResults.Length > 0)
        {
            IWebElement? element = xPathResults.Select(x => x.Get(xPathCondition)).FirstOrDefault(x => x is not null);

            if (element is null && !searchOptions.IsSafely)
            {
                throw ElementNotFoundException.Create(
                    new SearchFailureData
                    {
                        ElementName = executionData.Component.ComponentFullName,
                        By = xPathResults[0].CreateBy(xPathCondition),
                        SearchOptions = searchOptions
                    });
            }
            else
            {
                return element;
            }
        }
        else
        {
            return null;
        }
    }

    public IWebElement[] GetElements(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        var executionData = _executionDataCollector.Get(searchOptions);
        XPathComponentScopeFindResult[] xPathResults = _executor.Execute(executionData);

        foreach (var xPathResult in xPathResults)
        {
            SearchOptions quickSearchOptions = xPathResult.SearchOptions.Clone();
            quickSearchOptions.IsSafely = true;
            quickSearchOptions.Timeout = TimeSpan.Zero;

            xPathResult.SearchOptions = quickSearchOptions;
        }

        return xPathResults.Length > 0
            ? [.. xPathResults.Select(x => x.GetAll(xPathCondition)).Where(x => x.Count > 0).SelectMany(x => x)]
            : [];
    }

    public bool IsMissing(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        SearchOptions quickSearchOptions = searchOptions.Clone();
        quickSearchOptions.IsSafely = true;
        quickSearchOptions.Timeout = TimeSpan.Zero;

        StrategyScopeLocatorExecutionData executionData = _executionDataCollector.Get(quickSearchOptions);
        var driver = executionData.Component.Session.Driver;

        RetryWait wait = new(searchOptions.Timeout, searchOptions.RetryInterval);

        bool isMissing = wait.Until(() =>
        {
            XPathComponentScopeFindResult[] xPathResults = _executor.Execute(executionData);

            if (xPathResults.Length > 0)
            {
                Dictionary<By, ISearchContext> byScopePairs = xPathResults.ToDictionary(x => x.CreateBy(xPathCondition), x => x.ScopeSource);
                return driver.Try(TimeSpan.Zero).MissingAll(byScopePairs);
            }
            else
            {
                return true;
            }
        });

        if (!searchOptions.IsSafely && !isMissing)
        {
            throw ElementNotMissingException.Create(
                new SearchFailureData
                {
                    ElementName = executionData.Component.ComponentFullName,
                    SearchOptions = searchOptions
                });
        }
        else
        {
            return isMissing;
        }
    }
}
