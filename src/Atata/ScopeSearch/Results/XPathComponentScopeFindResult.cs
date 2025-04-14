namespace Atata;

public sealed class XPathComponentScopeFindResult : ComponentScopeFindResult
{
    private readonly ILogManager _log;

    public XPathComponentScopeFindResult(
        string xPath,
        ISearchContext scopeSource,
        SearchOptions searchOptions,
        UIComponent component)
    {
        Guard.ThrowIfNullOrEmpty(xPath);
        Guard.ThrowIfNull(scopeSource);
        Guard.ThrowIfNull(searchOptions);
        Guard.ThrowIfNull(component);

        XPath = xPath;
        ScopeSource = scopeSource;
        SearchOptions = searchOptions;
        _log = component.Log;
    }

    public string XPath { get; }

    public ISearchContext ScopeSource { get; }

    public SearchOptions SearchOptions { get; internal set; }

    public IWebElement Get(string? xPathCondition = null) =>
        ScopeSource.GetWithLogging(_log, CreateBy(xPathCondition))!;

    public ReadOnlyCollection<IWebElement> GetAll(string? xPathCondition = null) =>
        ScopeSource.GetAllWithLogging(_log, CreateBy(xPathCondition));

    public By CreateBy(string? xPathCondition)
    {
        string combinedXPath = CombineXPathWithCodition(xPathCondition);
        return By.XPath(combinedXPath).With(SearchOptions);
    }

    private string CombineXPathWithCodition(string? xPathCondition)
    {
        if (xPathCondition?.Length > 0)
        {
            StringBuilder xPathBuilder = new(XPath, XPath.Length + xPathCondition.Length + 1);

            if (xPathCondition[0] is not '[' and not '/')
                xPathBuilder.Append('/');

            xPathBuilder.Append(xPathCondition);
            return xPathBuilder.ToString();
        }
        else
        {
            return XPath;
        }
    }
}
