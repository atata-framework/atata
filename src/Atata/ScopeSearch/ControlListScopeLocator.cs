using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class ControlListScopeLocator : IScopeLocator
    {
        private readonly Func<SearchOptions, IEnumerable<IWebElement>> locateFunction;

        public ControlListScopeLocator(Func<SearchOptions, IEnumerable<IWebElement>> locateFunction)
        {
            this.locateFunction = locateFunction;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            IWebElement element = ATContext.Current.Driver.Try(searchOptions.Timeout, searchOptions.RetryInterval).Until(_ =>
            {
                return locateFunction(searchOptions).FirstOrDefault();
            });

            if (element == null && !searchOptions.IsSafely)
                throw ExceptionFactory.CreateForNotMissingElement();
            else
                return element;
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            return ATContext.Current.Driver.Try(searchOptions.Timeout, searchOptions.RetryInterval).Until(_ =>
            {
                return locateFunction(searchOptions).ToArray();
            });
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            bool isMissing = ATContext.Current.Driver.Try(searchOptions.Timeout, searchOptions.RetryInterval).Until(_ =>
            {
                return !locateFunction(searchOptions).Any();
            });

            if (!isMissing && !searchOptions.IsSafely)
                throw ExceptionFactory.CreateForNotMissingElement();
            else
                return isMissing;
        }
    }
}
