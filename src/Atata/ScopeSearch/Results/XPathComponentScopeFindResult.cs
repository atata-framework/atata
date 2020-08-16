using System.Collections.ObjectModel;
using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    public class XPathComponentScopeFindResult : ComponentScopeLocateResult
    {
        public XPathComponentScopeFindResult(string xPath, ISearchContext scopeSource, SearchOptions searchOptions)
        {
            XPath = xPath;
            ScopeSource = scopeSource;
            SearchOptions = searchOptions;
        }

        public string XPath { get; }

        public ISearchContext ScopeSource { get; }

        public SearchOptions SearchOptions { get; }

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
            StringBuilder xPathBuilder = new StringBuilder(XPath);

            if (!string.IsNullOrWhiteSpace(xPathCondition))
            {
                if (!xPathCondition.StartsWith("[") && !xPathCondition.StartsWith("/"))
                    xPathBuilder.Append("/");

                xPathBuilder.Append(xPathCondition);
            }

            return By.XPath(xPathBuilder.ToString()).With(SearchOptions);
        }
    }
}
