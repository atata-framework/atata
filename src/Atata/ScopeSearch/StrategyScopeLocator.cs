using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class StrategyScopeLocator : IScopeLocator
    {
        private readonly UIComponent component;
        private readonly IComponentScopeLocateStrategy strategy;
        private readonly ComponentScopeLocateOptions scopeLocateOptions;

        public StrategyScopeLocator(UIComponent component, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions)
        {
            this.component = component;
            this.strategy = strategy;
            this.scopeLocateOptions = scopeLocateOptions;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = ResolveSearchOptions(searchOptions);

            XPathComponentScopeLocateResult[] xPathResults = GetScopeLocateResults(searchOptions);
            if (xPathResults != null && xPathResults.Any())
            {
                IWebElement element = xPathResults.Select(x => x.Get(xPathCondition)).FirstOrDefault(x => x != null);

                if (element == null && !searchOptions.IsSafely)
                {
                    throw ExceptionFactory.CreateForNoSuchElement(
                        new SearchFailureData
                        {
                            ElementName = component.ComponentFullName,
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
            searchOptions = ResolveSearchOptions(searchOptions);

            XPathComponentScopeLocateResult[] xPathResults = GetScopeLocateResults(searchOptions);

            return xPathResults.Any()
                ? xPathResults.Select(x => x.GetAll(xPathCondition)).Where(x => x.Any()).SelectMany(x => x).ToArray()
                : new IWebElement[0];
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = ResolveSearchOptions(searchOptions);

            SearchOptions quickSearchOptions = SearchOptions.SafelyAtOnce();
            quickSearchOptions.Visibility = searchOptions.Visibility;

            bool isMissing = component.Driver.Try(searchOptions.Timeout, searchOptions.RetryInterval).Until(_ =>
            {
                XPathComponentScopeLocateResult[] xPathResults = GetScopeLocateResults(quickSearchOptions);
                if (xPathResults.Any())
                {
                    Dictionary<By, ISearchContext> byScopePairs = xPathResults.ToDictionary(x => x.CreateBy(xPathCondition), x => (ISearchContext)x.ScopeSource);
                    return component.Driver.Try(TimeSpan.Zero).MissingAll(byScopePairs);
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
                        ElementName = component.ComponentFullName,
                        SearchOptions = searchOptions
                    });
            }
            else
            {
                return isMissing;
            }
        }

        private SearchOptions ResolveSearchOptions(SearchOptions searchOptions)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            if (!searchOptions.IsVisibilitySet)
                searchOptions.Visibility = scopeLocateOptions.Visibility;

            return searchOptions;
        }

        private XPathComponentScopeLocateResult[] GetScopeLocateResults(SearchOptions searchOptions)
        {
            IWebElement scopeSource = component.ScopeSource.GetScopeElement(component);

            if (scopeSource == null && searchOptions.IsSafely)
                return new XPathComponentScopeLocateResult[0];

            return ExecuteStrategyAndResolveResults(strategy, scopeSource, scopeLocateOptions, searchOptions);
        }

        private XPathComponentScopeLocateResult[] ResolveScopeLocateResult(ComponentScopeLocateResult result, IWebElement scopeSource, SearchOptions searchOptions)
        {
            result.CheckNotNull(nameof(result));

            if (result is MissingComponentScopeLocateResult missingResult)
                return new XPathComponentScopeLocateResult[0];

            if (result is XPathComponentScopeLocateResult xPathResult)
                return new[] { xPathResult };

            if (result is SequalComponentScopeLocateResult sequalResult)
            {
                ComponentScopeLocateOptions nextScopeLocateOptions = sequalResult.ScopeLocateOptions ?? scopeLocateOptions;

                if (sequalResult.ScopeSources?.Count() == 1)
                {
                    return ExecuteStrategyAndResolveResults(sequalResult.Strategy, sequalResult.ScopeSources.First(), nextScopeLocateOptions, searchOptions);
                }
                else
                {
                    IEnumerable<IWebElement> nextScopeSources = sequalResult.ScopeSourceBy != null
                        ? scopeSource.GetAll(sequalResult.ScopeSourceBy.With(searchOptions))
                        : sequalResult.ScopeSources;

                    SearchOptions nextSearchOptions = SearchOptions.SafelyAtOnce();

                    var results = nextScopeSources.
                        Select(nextScopeSource => ExecuteStrategyAndResolveResults(sequalResult.Strategy, nextScopeSource, nextScopeLocateOptions, nextSearchOptions)).
                        Where(xPathResults => xPathResults != null).
                        SelectMany(xPathResults => xPathResults).
                        ToArray();

                    if (results.Any())
                    {
                        return results;
                    }
                    else if (searchOptions.IsSafely)
                    {
                        return new XPathComponentScopeLocateResult[0];
                    }
                    else
                    {
                        throw ExceptionFactory.CreateForNoSuchElement(
                            new SearchFailureData
                            {
                                ElementName = component.ComponentFullName,
                                By = sequalResult.ScopeSourceBy,
                                SearchOptions = searchOptions,
                                SearchContext = scopeSource
                            });
                    }
                }
            }

            throw new ArgumentException($"Unsupported {nameof(ComponentScopeLocateResult)} type: {result.GetType().FullName}", nameof(result));
        }

        private XPathComponentScopeLocateResult[] ExecuteStrategyAndResolveResults(IComponentScopeLocateStrategy strategy, IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            ComponentScopeLocateResult result = strategy.Find(scope, options, searchOptions);

            return ResolveScopeLocateResult(result, scope, searchOptions);
        }
    }
}
