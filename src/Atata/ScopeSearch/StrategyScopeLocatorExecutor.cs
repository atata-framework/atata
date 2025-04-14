﻿namespace Atata;

public class StrategyScopeLocatorExecutor : IStrategyScopeLocatorExecutor
{
    public static StrategyScopeLocatorExecutor Default { get; } = new();

    public XPathComponentScopeFindResult[] Execute(StrategyScopeLocatorExecutionData executionData)
    {
        ISearchContext scopeContext = executionData.ScopeSource.GetScopeContext(
            executionData.Component,
            SearchOptions.Safely(executionData.IsSafely));

        if (scopeContext is null)
            return [];

        foreach (var unit in executionData.LayerUnits)
        {
            XPathComponentScopeFindResult[] xPathResults = Execute(unit.Strategy, scopeContext, unit.ScopeFindOptions, unit.SearchOptions);

            if (xPathResults.Length == 0)
                return xPathResults;

            IWebElement element = xPathResults.Select(x => x.Get()).FirstOrDefault(x => x != null);

            if (element is null)
            {
                if (!executionData.IsSafely)
                {
                    XPathComponentScopeFindResult firstResult = xPathResults[0];

                    throw ElementExceptionFactory.CreateForNotFound(
                        new SearchFailureData
                        {
                            ElementName = $"layer of {executionData.Component.ComponentFullName}",
                            By = By.XPath(firstResult.XPath),
                            SearchOptions = firstResult.SearchOptions
                        });
                }
                else
                {
                    return [];
                }
            }

            scopeContext = unit.ScopeContextResolver.Resolve(element, executionData.Component.Session);
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
        Guard.ThrowIfNull(result);

        if (result is MissingComponentScopeFindResult)
            return [];

        if (result is XPathComponentScopeFindResult xPathResult)
            return [xPathResult];

        if (result is SubsequentComponentScopeFindResult subsequentResult)
        {
            ComponentScopeFindOptions nextScopeFindOptions = subsequentResult.ScopeFindOptions ?? scopeLocateOptions;

            if (subsequentResult.ScopeSources.Count == 1)
            {
                return Execute(subsequentResult.Strategy, subsequentResult.ScopeSources[0], nextScopeFindOptions, searchOptions);
            }
            else
            {
                IEnumerable<ISearchContext> nextScopeSources = subsequentResult.ScopeSourceBy is not null
                    ? scopeSource.GetAllWithLogging(
                        scopeLocateOptions.Component.Log,
                        subsequentResult.ScopeSourceBy.With(searchOptions))
                    : subsequentResult.ScopeSources;

                SearchOptions nextSearchOptions = SearchOptions.SafelyAtOnce();

                // TODO: When there are no results, do retry.
                var results = nextScopeSources
                    .Select(nextScopeSource => Execute(subsequentResult.Strategy, nextScopeSource, nextScopeFindOptions, nextSearchOptions))
                    .Where(xPathResults => xPathResults is not null)
                    .SelectMany(xPathResults => xPathResults)
                    .ToArray();

                if (results.Length > 0)
                {
                    return results;
                }
                else if (searchOptions.IsSafely)
                {
                    return [];
                }
                else
                {
                    throw ElementExceptionFactory.CreateForNotFound(
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
