namespace Atata;

public class DynamicScopeLocator : IScopeLocator
{
    private readonly Func<SearchOptions, IWebElement> _locateFunction;

    public DynamicScopeLocator(Func<SearchOptions, IWebElement> locateFunction) =>
        _locateFunction = locateFunction;

    public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new SearchOptions();

        return _locateFunction(searchOptions);
    }

    public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null) =>
        [GetElement(searchOptions, xPathCondition)];

    public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new SearchOptions();

        SearchOptions searchOptionsForElementGet = searchOptions.Clone();
        searchOptionsForElementGet.IsSafely = true;

        IWebElement element = GetElement(searchOptionsForElementGet, xPathCondition);
        if (element != null && !searchOptions.IsSafely)
            throw ElementExceptionFactory.CreateForNotMissing();
        else
            return element == null;
    }
}
