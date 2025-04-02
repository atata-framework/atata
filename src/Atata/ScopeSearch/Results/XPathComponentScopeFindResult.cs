namespace Atata;

public sealed class XPathComponentScopeFindResult : ComponentScopeFindResult
{
    private readonly ILogManager _log;

    public XPathComponentScopeFindResult(
        string xPath,
        ISearchContext scopeSource,
        SearchOptions searchOptions)
        : this(xPath, scopeSource, searchOptions, null)
    {
    }

    public XPathComponentScopeFindResult(
        string xPath,
        ISearchContext scopeSource,
        SearchOptions searchOptions,
        UIComponent component)
    {
        XPath = xPath;
        ScopeSource = scopeSource;
        SearchOptions = searchOptions;
        _log = component?.Log ?? AtataContext.Current?.Sessions.GetOrNull<WebDriverSession>()?.Log;
    }

    public string XPath { get; }

    public ISearchContext ScopeSource { get; }

    public SearchOptions SearchOptions { get; internal set; }

    public IWebElement Get(string xPathCondition = null) =>
        ScopeSource.GetWithLogging(_log, CreateBy(xPathCondition));

    public ReadOnlyCollection<IWebElement> GetAll(string xPathCondition = null) =>
        ScopeSource.GetAllWithLogging(_log, CreateBy(xPathCondition));

    public By CreateBy(string xPathCondition)
    {
        StringBuilder xPathBuilder = new StringBuilder(XPath);

        if (!string.IsNullOrWhiteSpace(xPathCondition))
        {
            if (xPathCondition[0] is not '[' and not '/')
                xPathBuilder.Append('/');

            xPathBuilder.Append(xPathCondition);
        }

        return By.XPath(xPathBuilder.ToString()).With(SearchOptions);
    }
}
