using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class StrategyScopeLocatorExecutor : IStrategyScopeLocatorExecutor
    {
        public static StrategyScopeLocatorExecutor Default { get; } = new StrategyScopeLocatorExecutor();

        public XPathComponentScopeFindResult[] Execute(StrategyScopeLocatorExecutionData executionData)
        {
            ISearchContext scopeContext = executionData.ScopeSource.GetScopeContext(
                executionData.Component,
                SearchOptions.Safely(executionData.IsSafely));

            if (scopeContext is null)
                return new XPathComponentScopeFindResult[0];

            foreach (var unit in executionData.LayerUnits)
            {
                XPathComponentScopeFindResult[] xPathResults = Execute(unit.Strategy, scopeContext, unit.ScopeFindOptions, unit.SearchOptions);

                if (!xPathResults.Any())
                    return xPathResults;

                IWebElement element = xPathResults.Select(x => x.Get()).FirstOrDefault(x => x != null);

                if (element is null)
                {
                    if (!executionData.IsSafely)
                    {
                        XPathComponentScopeFindResult firstResult = xPathResults[0];

                        throw ExceptionFactory.CreateForNoSuchElement(
                            new SearchFailureData
                            {
                                ElementName = $"layer of {executionData.Component.ComponentFullName}",
                                By = By.XPath(firstResult.XPath),
                                SearchOptions = firstResult.SearchOptions
                            });
                    }
                    else
                    {
                        return new XPathComponentScopeFindResult[0];
                    }
                }

                scopeContext = unit.ScopeContextResolver.Resolve(element);
            }

            return Execute(executionData.FinalUnit.Strategy, scopeContext, executionData.FinalUnit.ScopeFindOptions, executionData.FinalUnit.SearchOptions);
        }

        private static XPathComponentScopeFindResult[] Execute(IComponentScopeFindStrategy strategy, ISearchContext scope, ComponentScopeFindOptions scopeLocateOptions, SearchOptions searchOptions)
        {
            ComponentScopeFindResult result = strategy.Find(scope, scopeLocateOptions, searchOptions);

            return ResolveResult(result, scope, scopeLocateOptions, searchOptions);
        }

        private static XPathComponentScopeFindResult[] ResolveResult(
            ComponentScopeFindResult result,
            ISearchContext scopeSource,
            ComponentScopeFindOptions scopeLocateOptions,
            SearchOptions searchOptions)
        {
            result.CheckNotNull(nameof(result));

            if (result is MissingComponentScopeFindResult)
                return new XPathComponentScopeFindResult[0];

            if (result is XPathComponentScopeFindResult xPathResult)
                return new[] { xPathResult };

            if (result is SubsequentComponentScopeFindResult subsequentResult)
            {
                ComponentScopeFindOptions nextScopeFindOptions = subsequentResult.ScopeFindOptions ?? scopeLocateOptions;

                if (subsequentResult.ScopeSources?.Count() == 1)
                {
                    return Execute(subsequentResult.Strategy, subsequentResult.ScopeSources.First(), nextScopeFindOptions, searchOptions);
                }
                else
                {
                    IEnumerable<ISearchContext> nextScopeSources = subsequentResult.ScopeSourceBy != null
                        ? scopeSource.GetAllWithLogging(subsequentResult.ScopeSourceBy.With(searchOptions))
                        : subsequentResult.ScopeSources;

                    SearchOptions nextSearchOptions = SearchOptions.SafelyAtOnce();

                    // TODO: When there are no results, do retry.
                    var results = nextScopeSources
                        .Select(nextScopeSource => Execute(subsequentResult.Strategy, nextScopeSource, nextScopeFindOptions, nextSearchOptions))
                        .Where(xPathResults => xPathResults != null)
                        .SelectMany(xPathResults => xPathResults)
                        .ToArray();

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
                                ElementName = scopeLocateOptions.Component.ComponentFullName,
                                By = subsequentResult.ScopeSourceBy,
                                SearchOptions = searchOptions,
                                SearchContext = scopeSource
                            });
                    }
                }
            }

            throw new ArgumentException($"Unsupported {nameof(ComponentScopeFindResult)} type: {result.GetType().FullName}", nameof(result));
        }
    }
}
