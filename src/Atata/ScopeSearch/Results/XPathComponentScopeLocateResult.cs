using System;
using OpenQA.Selenium;

namespace Atata
{
    [Obsolete("Use XPathComponentScopeFindResult class instead.")] // Obsolete since v1.5.0.
    public class XPathComponentScopeLocateResult : XPathComponentScopeFindResult
    {
        public XPathComponentScopeLocateResult(string xPath, IWebElement scopeSource, SearchOptions searchOptions)
            : base(xPath, scopeSource, searchOptions)
        {
        }

        public new IWebElement ScopeSource => (IWebElement)base.ScopeSource;
    }
}
