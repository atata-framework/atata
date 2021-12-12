using System.Collections.ObjectModel;
using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    public sealed class XPathComponentScopeFindResult : ComponentScopeFindResult
    {
        public XPathComponentScopeFindResult(string xPath, ISearchContext scopeSource, SearchOptions searchOptions)
        {
            XPath = xPath;
            ScopeSource = scopeSource;
            SearchOptions = searchOptions;
        }

        public string XPath { get; }

        public ISearchContext ScopeSource { get; }

        public SearchOptions SearchOptions { get; internal set; }

        public IWebElement Get(string xPathCondition = null) =>
            ScopeSource.GetWithLogging(CreateBy(xPathCondition));

        public ReadOnlyCollection<IWebElement> GetAll(string xPathCondition = null) =>
            ScopeSource.GetAllWithLogging(CreateBy(xPathCondition));

        public By CreateBy(string xPathCondition)
        {
            StringBuilder xPathBuilder = new StringBuilder(XPath);

            if (!string.IsNullOrWhiteSpace(xPathCondition))
            {
                if (xPathCondition[0] != '[' && xPathCondition[0] != '/')
                    xPathBuilder.Append('/');

                xPathBuilder.Append(xPathCondition);
            }

            return By.XPath(xPathBuilder.ToString()).With(SearchOptions);
        }
    }
}
