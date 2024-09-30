namespace Atata;

public class PlainScopeLocator : IScopeLocator
{
    private readonly WebDriverSession _session;

    private readonly Func<By> _byCreator;

    private By _by;

    public PlainScopeLocator(WebDriverSession session, By by)
    {
        _session = session;
        _by = by.CheckNotNull(nameof(by));
    }

    public PlainScopeLocator(WebDriverSession session, Func<By> byCreator)
    {
        _session = session;
        _byCreator = byCreator.CheckNotNull(nameof(byCreator));
    }

    private By By =>
        _by ??= _byCreator();

    public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new SearchOptions();

        return _session.Driver.GetWithLogging(_session.Log, By.With(searchOptions));
    }

    public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new SearchOptions();

        return [.. _session.Driver.GetAllWithLogging(_session.Log, By.With(searchOptions))];
    }

    public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
    {
        searchOptions ??= new SearchOptions();

        return _session.Driver.Missing(By.With(searchOptions));
    }
}
