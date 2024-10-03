namespace Atata;

public class ControlListScopeLocator : IScopeLocator
{
    private readonly WebDriverSession _session;

    private readonly Func<SearchOptions, IEnumerable<IWebElement>> _predicate;

    public ControlListScopeLocator(WebDriverSession session, Func<SearchOptions, IEnumerable<IWebElement>> predicate)
    {
        _session = session;
        _predicate = predicate;
    }

    public string ElementName { get; set; }

    public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new SearchOptions();

        IWebElement element = _session.Driver
            .Try(searchOptions.Timeout, searchOptions.RetryInterval)
            .Until(_ => _predicate(searchOptions).FirstOrDefault());

        if (element == null && !searchOptions.IsSafely)
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

    public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new SearchOptions();

        return _session.Driver
            .Try(searchOptions.Timeout, searchOptions.RetryInterval)
            .Until(_ => _predicate(searchOptions).ToArray());
    }

    public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new SearchOptions();

        bool isMissing = _session.Driver
            .Try(searchOptions.Timeout, searchOptions.RetryInterval)
            .Until(_ => !_predicate(searchOptions).Any());

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
