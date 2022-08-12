using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class ControlListScopeLocator : IScopeLocator
    {
        private readonly Func<SearchOptions, IEnumerable<IWebElement>> _predicate;

        public ControlListScopeLocator(Func<SearchOptions, IEnumerable<IWebElement>> predicate) =>
            _predicate = predicate;

        public string ElementName { get; set; }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            IWebElement element = AtataContext.Current.Driver
                .Try(searchOptions.Timeout, searchOptions.RetryInterval)
                .Until(_ => _predicate(searchOptions).FirstOrDefault());

            if (element == null && !searchOptions.IsSafely)
            {
                throw ExceptionFactory.CreateForNoSuchElement(
                    new SearchFailureData
                    {
                        ElementName = ElementName,
                        SearchOptions = searchOptions
                    });
            }
            else
            {
                return element;
            }
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            return AtataContext.Current.Driver
                .Try(searchOptions.Timeout, searchOptions.RetryInterval)
                .Until(_ => _predicate(searchOptions).ToArray());
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            bool isMissing = AtataContext.Current.Driver
                .Try(searchOptions.Timeout, searchOptions.RetryInterval)
                .Until(_ => !_predicate(searchOptions).Any());

            if (!isMissing && !searchOptions.IsSafely)
            {
                throw ExceptionFactory.CreateForNotMissingElement(
                    new SearchFailureData
                    {
                        ElementName = ElementName,
                        SearchOptions = searchOptions
                    });
            }
            else
            {
                return isMissing;
            }
        }
    }
}
