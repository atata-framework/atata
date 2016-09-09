using System.Collections.ObjectModel;
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

        public IWebElement Get(string xPathCondition = null)
        {
            return ScopeSource.Get(CreateBy(xPathCondition));
        }

        public ReadOnlyCollection<IWebElement> GetAll(string xPathCondition = null)
        {
            return ScopeSource.GetAll(CreateBy(xPathCondition));
        }

        public By CreateBy(string xPathCondition)
        {
            return By.XPath(XPath + xPathCondition).With(SearchOptions);
        }
    }
}
