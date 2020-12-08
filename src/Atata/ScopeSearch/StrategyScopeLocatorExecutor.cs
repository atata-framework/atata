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
                XPathComponentScopeFindResult[] xPathResults = Execute(unit.Strategy, scopeContext, unit.ScopeLocateOptions, unit.SearchOptions);

                if (!xPathResults.Any())
                    return xPathResults;

                IWebElement element = xPathResults.Select(x => x.Get()).FirstOrDefault(x => x != null);

                if (element is null)
                {
                    if (!executionData.IsSafely)
                    {
                        XPathComponentScopeFindResult firstResult = xPathResults.First();

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

            return Execute(executionData.FinalUnit.Strategy, scopeContext, executionData.FinalUnit.ScopeLocateOptions, executionData.FinalUnit.SearchOptions);
        }

        private static XPathComponentScopeFindResult[] Execute(object strategy, ISearchContext scope, ComponentScopeLocateOptions scopeLocateOptions, SearchOptions searchOptions)
        {
            ComponentScopeLocateResult result = ExecuteToGetResult(strategy, scope, scopeLocateOptions, searchOptions);

            return ResolveResult(result, scope, scopeLocateOptions, searchOptions);
        }

        private static ComponentScopeLocateResult ExecuteToGetResult(object strategy, ISearchContext scope, ComponentScopeLocateOptions scopeLocateOptions, SearchOptions searchOptions)
        {
            // TODO: Remove first "if" and last "else" in Atata v2.0.0, as strategy argument should be of IComponentScopeFindStrategy type.
#pragma warning disable CS0618 // Type or member is obsolete
            if (strategy is IComponentScopeLocateStrategy componentScopeLocateStrategy)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                IWebElement scopeElement = (scope as IWebElement)
                    ?? scope.GetWithLogging(By.TagName("body").With(new SearchOptions()));

                return componentScopeLocateStrategy.Find(scopeElement, scopeLocateOptions, searchOptions);
            }
            else if (strategy is IComponentScopeFindStrategy componentScopeFindStrategy)
            {
                return componentScopeFindStrategy.Find(scope, scopeLocateOptions, searchOptions);
            }
            else
            {
                throw new ArgumentException(
                    $"Unsupported {strategy.GetType().FullName} type of {nameof(strategy)}. Strategy should be either {nameof(IComponentScopeFindStrategy)} or IComponentScopeLocateStrategy.",
                    nameof(strategy));
            }
        }

        private static XPathComponentScopeFindResult[] ResolveResult(
            ComponentScopeLocateResult result,
            ISearchContext scopeSource,
            ComponentScopeLocateOptions scopeLocateOptions,
            SearchOptions searchOptions)
        {
            result.CheckNotNull(nameof(result));

            if (result is MissingComponentScopeFindResult missingResult)
                return new XPathComponentScopeFindResult[0];

            if (result is XPathComponentScopeFindResult xPathResult)
                return new[] { xPathResult };

            if (result is SubsequentComponentScopeFindResult subsequentResult)
            {
                ComponentScopeLocateOptions nextScopeLocateOptions = subsequentResult.ScopeLocateOptions ?? scopeLocateOptions;

                if (subsequentResult.ScopeSources?.Count() == 1)
                {
                    return Execute(subsequentResult.Strategy, subsequentResult.ScopeSources.First(), nextScopeLocateOptions, searchOptions);
                }
                else
                {
                    IEnumerable<ISearchContext> nextScopeSources = subsequentResult.ScopeSourceBy != null
                        ? scopeSource.GetAllWithLogging(subsequentResult.ScopeSourceBy.With(searchOptions))
                        : subsequentResult.ScopeSources;

                    SearchOptions nextSearchOptions = SearchOptions.SafelyAtOnce();

                    // TODO: When there are no results, do retry.
                    var results = nextScopeSources.
                        Select(nextScopeSource => Execute(subsequentResult.Strategy, nextScopeSource, nextScopeLocateOptions, nextSearchOptions)).
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
                                ElementName = scopeLocateOptions.Component.ComponentFullName,
                                By = subsequentResult.ScopeSourceBy,
                                SearchOptions = searchOptions,
                                SearchContext = scopeSource
                            });
                    }
                }
            }

            throw new ArgumentException($"Unsupported {nameof(ComponentScopeLocateResult)} type: {result.GetType().FullName}", nameof(result));
        }
    }
}
