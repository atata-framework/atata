namespace Atata;

public class ControlListScopeLocator : IScopeLocator
{
    private readonly Func<SearchOptions, IEnumerable<IWebElement>> _predicate;

    public ControlListScopeLocator(Func<SearchOptions, IEnumerable<IWebElement>> predicate) =>
        _predicate = predicate;

    public string? ElementName { get; set; }

    public IWebElement? GetElement(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        RetryWait wait = new(searchOptions.Timeout, searchOptions.RetryInterval);
        IWebElement? element = null;

        wait.Until(() =>
        {
            element = _predicate(searchOptions).FirstOrDefault();
            return element is not null;
        });

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

    public IWebElement[] GetElements(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        RetryWait wait = new(searchOptions.Timeout, searchOptions.RetryInterval);
        IWebElement[]? elements = null;

        wait.Until(() =>
        {
            elements = _predicate(searchOptions)?.ToArray();
            return elements?.Length > 0;
        });

        return elements ?? [];
    }

    public bool IsMissing(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        RetryWait wait = new(searchOptions.Timeout, searchOptions.RetryInterval);

        bool isMissing = wait.Until(() => !_predicate(searchOptions).Any());

        if (!isMissing && !searchOptions.IsSafely)
        {
            throw ElementExceptionFactory.CreateForNotMissing(
                new SearchFailureData
                {
                    ElementName = ElementName,
                    SearchOptions = searchOptions
                });
        }
        else
        {
            return isMissing;
        }
    }
}
