using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class ControlListScopeLocator : IScopeLocator
    {
        private readonly Func<SearchOptions, IEnumerable<IWebElement>> predicate;

        public ControlListScopeLocator(Func<SearchOptions, IEnumerable<IWebElement>> predicate)
        {
            this.predicate = predicate;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            IWebElement element = ATContext.Current.Driver.Try(searchOptions.Timeout, searchOptions.RetryInterval).Until(_ =>
            {
                return predicate(searchOptions).FirstOrDefault();
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
                return predicate(searchOptions).ToArray();
            });
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            bool isMissing = ATContext.Current.Driver.Try(searchOptions.Timeout, searchOptions.RetryInterval).Until(_ =>
            {
                return !predicate(searchOptions).Any();
            });

            if (!isMissing && !searchOptions.IsSafely)
                throw ExceptionFactory.CreateForNotMissingElement();
            else
                return isMissing;
        }
    }
}
