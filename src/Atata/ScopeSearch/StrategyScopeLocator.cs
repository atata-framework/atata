using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class StrategyScopeLocator : IScopeLocator
    {
        private readonly IStrategyScopeLocatorExecutionDataCollector executionDataCollector;

        private readonly IStrategyScopeLocatorExecutor executor;

        public StrategyScopeLocator(
            IStrategyScopeLocatorExecutionDataCollector executionDataCollector,
            IStrategyScopeLocatorExecutor executor)
        {
            this.executionDataCollector = executionDataCollector;
            this.executor = executor;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            var executionData = executionDataCollector.Get(searchOptions);
            XPathComponentScopeFindResult[] xPathResults = executor.Execute(executionData);

            if (xPathResults.Any())
            {
                IWebElement element = xPathResults.Select(x => x.Get(xPathCondition)).FirstOrDefault(x => x != null);

                if (element == null && !searchOptions.IsSafely)
                {
                    throw ExceptionFactory.CreateForNoSuchElement(
                        new SearchFailureData
                        {
                            ElementName = executionData.Component.ComponentFullName,
                            By = By.XPath(xPathResults.First().XPath + xPathCondition),
                            SearchOptions = searchOptions
                        });
                }
                else
                {
                    return element;
                }
            }
            else
            {
                return null;
            }
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            var executionData = executionDataCollector.Get(searchOptions);
            XPathComponentScopeFindResult[] xPathResults = executor.Execute(executionData);

            return xPathResults.Any()
                ? xPathResults.Select(x => x.GetAll(xPathCondition)).Where(x => x.Any()).SelectMany(x => x).ToArray()
                : new IWebElement[0];
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            SearchOptions quickSearchOptions = searchOptions.Clone();
            quickSearchOptions.IsSafely = true;
            quickSearchOptions.Timeout = TimeSpan.Zero;

            var driver = AtataContext.Current.Driver;
            StrategyScopeLocatorExecutionData executionData = executionDataCollector.Get(quickSearchOptions);

            bool isMissing = driver.Try(searchOptions.Timeout, searchOptions.RetryInterval).Until(_ =>
            {
                XPathComponentScopeFindResult[] xPathResults = executor.Execute(executionData);

                if (xPathResults.Any())
                {
                    Dictionary<By, ISearchContext> byScopePairs = xPathResults.ToDictionary(x => x.CreateBy(xPathCondition), x => x.ScopeSource);
                    return driver.Try(TimeSpan.Zero).MissingAll(byScopePairs);
                }
                else
                {
                    return true;
                }
            });

            if (!searchOptions.IsSafely && !isMissing)
            {
                throw ExceptionFactory.CreateForNotMissingElement(
                    new SearchFailureData
                    {
                        ElementName = executionData.Component.ComponentFullName,
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
