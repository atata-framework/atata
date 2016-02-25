using OpenQA.Selenium;
namespace Atata
{
    public class XPathComponentScopeLocateResult : ComponentScopeLocateResult
    {
        public XPathComponentScopeLocateResult(string xPath, IWebElement scopeSource, SearchOptions searchOptions)
        {
            XPath = xPath;
            ScopeSource = scopeSource;
            SearchOptions = searchOptions;
        }

        public string XPath { get; private set; }
        public IWebElement ScopeSource { get; private set; }
        public SearchOptions SearchOptions { get; private set; }

        public IWebElement Get(string extraXPath = null)
        {
            return ScopeSource.Get(By.XPath(XPath + extraXPath).With(SearchOptions));
        }
    }
}
