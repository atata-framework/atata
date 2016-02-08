using OpenQA.Selenium;
using System;

namespace Atata
{
    public class DynamicScopeLocator : IScopeLocator
    {
        private readonly Func<SearchOptions, IWebElement> locateFunction;

        public DynamicScopeLocator(Func<SearchOptions, IWebElement> locateFunction)
        {
            this.locateFunction = locateFunction;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string extraXPath = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            return locateFunction(searchOptions);
        }
    }
}
