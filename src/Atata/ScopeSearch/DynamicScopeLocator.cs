using System;
using OpenQA.Selenium;

namespace Atata
{
    public class DynamicScopeLocator : IScopeLocator
    {
        private readonly Func<SearchOptions, IWebElement> locateFunction;

        public DynamicScopeLocator(Func<SearchOptions, IWebElement> locateFunction)
        {
            this.locateFunction = locateFunction;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            return locateFunction(searchOptions);
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            return new[] { GetElement(searchOptions, xPathCondition) };
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            SearchOptions searchOptionsForElementGet = searchOptions.Clone();
            searchOptionsForElementGet.IsSafely = true;

            IWebElement element = GetElement(searchOptionsForElementGet, xPathCondition);
            if (element != null && !searchOptions.IsSafely)
                throw ExceptionFactory.CreateForNotMissingElement();
            else
                return element == null;
        }
    }
}
