namespace Atata;

public class PlainScopeLocator : IScopeLocator
{
    private readonly WebDriverSession _session;

    private readonly ISearchContext? _searchContext;

    private readonly Func<ISearchContext>? _searchContextGetter;

    private readonly Func<By>? _byCreator;

    private By? _by;

    public PlainScopeLocator(WebDriverSession session, By by)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNull(by);

        _session = session;
        _searchContext = session.Driver;
        _by = by;
    }

    public PlainScopeLocator(WebDriverSession session, Func<By> byCreator)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNull(byCreator);

        _session = session;
        _searchContext = session.Driver;
        _byCreator = byCreator;
    }

    public PlainScopeLocator(WebDriverSession session, ISearchContext searchContext, By by)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNull(searchContext);
        Guard.ThrowIfNull(by);

        _session = session;
        _searchContext = searchContext;
        _by = by;
    }

    public PlainScopeLocator(WebDriverSession session, ISearchContext searchContext, Func<By> byCreator)
    {
        Guard.ThrowIfNull(session);
        Guard.ThrowIfNull(searchContext);
        Guard.ThrowIfNull(byCreator);

        _session = session;
        _searchContext = searchContext;
        _byCreator = byCreator;
    }

    public PlainScopeLocator(UIComponent component, By by)
    {
        Guard.ThrowIfNull(component);
        Guard.ThrowIfNull(by);

        _session = component.Session;
        _searchContextGetter = () => component.ScopeContext;
        _by = by;
    }

    public PlainScopeLocator(UIComponent component, Func<By> byCreator)
    {
        Guard.ThrowIfNull(component);
        Guard.ThrowIfNull(byCreator);

        _session = component.Session;
        _searchContextGetter = () => component.ScopeContext;
        _byCreator = byCreator;
    }

    private By By =>
        _by ??= _byCreator!.Invoke();

    private ISearchContext SearchContext =>
        _searchContext ?? _searchContextGetter!.Invoke();

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
