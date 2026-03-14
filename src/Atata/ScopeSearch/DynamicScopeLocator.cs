namespace Atata;

public class DynamicScopeLocator : IScopeLocator
{
    private readonly Func<SearchOptions, IWebElement> _locateFunction;

    public DynamicScopeLocator(Func<SearchOptions, IWebElement> locateFunction)
        : this(locateFunction, null)
    {
    }

    public DynamicScopeLocator(Func<SearchOptions, IWebElement> locateFunction, string elementName)
    {
        _locateFunction = locateFunction;
        ElementName = elementName;
    }

    public string ElementName { get; }

    public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new();

        IWebElement element = _locateFunction(searchOptions);

        if (element is null && !searchOptions.IsSafely)
        {
            throw ElementExceptionFactory.CreateForNotFound(
                new SearchFailureData
                {
                    ElementName = ElementName,
                    SearchOptions = searchOptions
                });
        }
        else
        {
            return element;
        }
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
