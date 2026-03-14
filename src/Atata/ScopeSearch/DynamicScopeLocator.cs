namespace Atata;

public class DynamicScopeLocator : IScopeLocator
{
    private readonly Func<SearchOptions, IWebElement?> _locateFunction;

    public DynamicScopeLocator(Func<SearchOptions, IWebElement?> locateFunction, string? elementName = null)
    {
        _locateFunction = locateFunction;
        ElementName = elementName;
    }

    public string? ElementName { get; }

    public IWebElement? GetElement(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        IWebElement? element = _locateFunction(searchOptions);

        if (element is null && !searchOptions.IsSafely)
        {
            throw ElementNotFoundException.Create(
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

    public IWebElement[] GetElements(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        IWebElement? element = GetElement(searchOptions, xPathCondition);

        return element is null
            ? []
            : [element];
    }

    public bool IsMissing(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        SearchOptions searchOptionsForElementGet = searchOptions.Clone();
        searchOptionsForElementGet.IsSafely = true;

        IWebElement? element = GetElement(searchOptionsForElementGet, xPathCondition);

        if (element is not null && !searchOptions.IsSafely)
            throw ElementNotMissingException.Create();
        else
            return element is null;
    }
}
