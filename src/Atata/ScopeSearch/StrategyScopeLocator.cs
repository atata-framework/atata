using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class StrategyScopeLocator : IScopeLocator
    {
        private readonly UIComponent component;

        public StrategyScopeLocator(UIComponent component)
        {
            this.component = component;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            FindAttribute findAttribute = component.Metadata.ResolveFindAttribute();
            ComponentScopeLocateOptions scopeLocateOptions = ComponentScopeLocateOptions.CreateFromMetadata(component.Metadata);

            searchOptions = ResolveSearchOptions(searchOptions, scopeLocateOptions.Visibility);

            XPathComponentScopeFindResult[] xPathResults = GetScopeLocateResults(findAttribute, scopeLocateOptions, searchOptions);

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
            FindAttribute findAttribute = component.Metadata.ResolveFindAttribute();
            ComponentScopeLocateOptions scopeLocateOptions = ComponentScopeLocateOptions.CreateFromMetadata(component.Metadata);

            searchOptions = ResolveSearchOptions(searchOptions, scopeLocateOptions.Visibility);

            XPathComponentScopeFindResult[] xPathResults = GetScopeLocateResults(findAttribute, scopeLocateOptions, searchOptions);

            return xPathResults.Any()
                ? xPathResults.Select(x => x.GetAll(xPathCondition)).Where(x => x.Any()).SelectMany(x => x).ToArray()
                : new IWebElement[0];
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            FindAttribute findAttribute = component.Metadata.ResolveFindAttribute();
            ComponentScopeLocateOptions scopeLocateOptions = ComponentScopeLocateOptions.CreateFromMetadata(component.Metadata);

            searchOptions = ResolveSearchOptions(searchOptions, scopeLocateOptions.Visibility);

            SearchOptions quickSearchOptions = SearchOptions.SafelyAtOnce();
            quickSearchOptions.Visibility = searchOptions.Visibility;

            bool isMissing = component.Driver.Try(searchOptions.Timeout, searchOptions.RetryInterval).Until(_ =>
            {
                XPathComponentScopeFindResult[] xPathResults = GetScopeLocateResults(findAttribute, scopeLocateOptions, quickSearchOptions);

                if (xPathResults.Any())
                {
                    Dictionary<By, ISearchContext> byScopePairs = xPathResults.ToDictionary(x => x.CreateBy(xPathCondition), x => x.ScopeSource);
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

        private SearchOptions ResolveSearchOptions(SearchOptions searchOptions, Visibility defaultVisibility)
        {
            searchOptions = searchOptions ?? new SearchOptions();

            if (!searchOptions.IsVisibilitySet)
                searchOptions.Visibility = defaultVisibility;

            return searchOptions;
        }

        private XPathComponentScopeFindResult[] GetScopeLocateResults(FindAttribute findAttribute, ComponentScopeLocateOptions scopeLocateOptions, SearchOptions searchOptions)
        {
            object strategy = findAttribute.CreateStrategy();

            ISearchContext scopeSource = strategy is IComponentScopeFindStrategy
                ? findAttribute.ScopeSource.GetScopeContext(component)
#pragma warning disable CS0618 // Type or member is obsolete
                : findAttribute.ScopeSource.GetScopeElement(component);
#pragma warning restore CS0618 // Type or member is obsolete

            if (scopeSource == null && searchOptions.IsSafely)
                return new XPathComponentScopeFindResult[0];

            return ExecuteStrategyAndResolveResults(strategy, scopeSource, scopeLocateOptions, searchOptions);
        }

        private XPathComponentScopeFindResult[] ResolveScopeLocateResult(ComponentScopeLocateResult result, ISearchContext scopeSource, ComponentScopeLocateOptions scopeLocateOptions, SearchOptions searchOptions)
        {
            result.CheckNotNull(nameof(result));

            if (result is MissingComponentScopeFindResult missingResult)
                return new XPathComponentScopeFindResult[0];

            if (result is XPathComponentScopeFindResult xPathResult)
                return new[] { xPathResult };

            if (result is SequalComponentScopeFindResult sequalResult)
            {
                ComponentScopeLocateOptions nextScopeLocateOptions = sequalResult.ScopeLocateOptions ?? scopeLocateOptions;

                if (sequalResult.ScopeSources?.Count() == 1)
                {
                    return ExecuteStrategyAndResolveResults(sequalResult.Strategy, sequalResult.ScopeSources.First(), nextScopeLocateOptions, searchOptions);
                }
                else
                {
                    IEnumerable<ISearchContext> nextScopeSources = sequalResult.ScopeSourceBy != null
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
                        return new XPathComponentScopeFindResult[0];
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

        private XPathComponentScopeFindResult[] ExecuteStrategyAndResolveResults(object strategy, ISearchContext scope, ComponentScopeLocateOptions scopeLocateOptions, SearchOptions searchOptions)
        {
            ComponentScopeLocateResult result =
#pragma warning disable CS0618 // Type or member is obsolete
                strategy is IComponentScopeLocateStrategy componentScopeLocateStrategy
#pragma warning restore CS0618 // Type or member is obsolete
                ? componentScopeLocateStrategy.Find((IWebElement)scope, scopeLocateOptions, searchOptions)
                : strategy is IComponentScopeFindStrategy componentScopeFindStrategy
                ? componentScopeFindStrategy.Find(scope, scopeLocateOptions, searchOptions)
                : throw new ArgumentException($"Unsupported {strategy.GetType().FullName} type of strategy. Strategy should be either {nameof(IComponentScopeFindStrategy)} or IComponentScopeLocateStrategy.", nameof(strategy));

            return ResolveScopeLocateResult(result, scope, scopeLocateOptions, searchOptions);
        }
    }
}
