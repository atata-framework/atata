namespace Atata;

public class PlainScopeLocator : IScopeLocator
{
    private readonly WebDriverSession _session;

    private readonly Func<By>? _byCreator;

    private By? _by;

    public PlainScopeLocator(WebDriverSession session, By by)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNull(by);

        _session = session;
        _by = by;
        SearchContext = session.Driver;
    }

    public PlainScopeLocator(WebDriverSession session, Func<By> byCreator)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNull(byCreator);

        _session = session;
        _byCreator = byCreator;
        SearchContext = session.Driver;
    }

    private By By =>
        _by ??= _byCreator!.Invoke();

    public ISearchContext SearchContext { get; set; }

    public IWebElement? GetElement(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        return SearchContext.GetWithLogging(_session.Log, By.With(searchOptions));
    }

    public IWebElement[] GetElements(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        return [.. SearchContext.GetAllWithLogging(_session.Log, By.With(searchOptions))];
    }

    public bool IsMissing(SearchOptions? searchOptions = null, string? xPathCondition = null)
    {
        searchOptions ??= new();

        return SearchContext.Missing(By.With(searchOptions));
    }
}
