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
            searchOptions = searchOptions ?? SearchOptions.Unsafely();

            XPathComponentScopeLocateResult[] xPathResults = GetScopeLocateResults(searchOptions);
            if (xPathResults != null && xPathResults.Any())
            {
                IWebElement element = xPathResults.Select(x => x.Get(xPathCondition)).FirstOrDefault(x => x != null);
                if (element == null && !searchOptions.IsSafely)
                    throw ExceptionFactory.CreateForNoSuchElement(by: By.XPath(xPathResults.First().XPath + xPathCondition));
                else
                    return element;
            }
            else
            {
                return null;
            }
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Unsafely();

            XPathComponentScopeLocateResult[] xPathResults = GetScopeLocateResults(searchOptions);
            if (xPathResults.Any())
                return xPathResults.Select(x => x.GetAll(xPathCondition)).Where(x => x.Any()).SelectMany(x => x).ToArray();
            else
                return new IWebElement[0];
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Unsafely();

            XPathComponentScopeLocateResult[] xPathResults = GetScopeLocateResults(searchOptions);
            if (xPathResults.Any())
            {
                Dictionary<By, ISearchContext> byScopePairs = xPathResults.ToDictionary(x => x.CreateBy(xPathCondition), x => (ISearchContext)x.ScopeSource);
                return component.Driver.Try().MissingAll(byScopePairs);
            }
            else
            {
                return true;
            }
        }

        private XPathComponentScopeLocateResult[] GetScopeLocateResults(SearchOptions searchOptions)
        {
            IWebElement scopeSource = component.ScopeSource.GetScopeElement(component);

            if (scopeSource == null && searchOptions.IsSafely)
                return null;

            ComponentScopeLocateResult result = strategy.Find(scopeSource, scopeLocateOptions, searchOptions);

            return ResolveScopeLocateResults(result, scopeSource, searchOptions);
        }

        private XPathComponentScopeLocateResult[] ResolveScopeLocateResults(ComponentScopeLocateResult result, IWebElement scopeSource, SearchOptions searchOptions)
        {
            result.CheckNotNull("result");

            MissingComponentScopeLocateResult missingResult = result as MissingComponentScopeLocateResult;
            if (missingResult != null)
                return null;

            XPathComponentScopeLocateResult xPathResult = result as XPathComponentScopeLocateResult;
            if (xPathResult != null)
                return new[] { xPathResult };

            SequalComponentScopeLocateResult sequalResult = result as SequalComponentScopeLocateResult;
            if (sequalResult != null)
            {
                ComponentScopeLocateOptions nextScopeLocateOptions = sequalResult.ScopeLocateOptions ?? scopeLocateOptions;

                if (sequalResult.ScopeSource != null)
                {
                    ComponentScopeLocateResult nextResult = sequalResult.Strategy.Find(sequalResult.ScopeSource, nextScopeLocateOptions, searchOptions);
                    return ResolveScopeLocateResults(nextResult, sequalResult.ScopeSource, searchOptions);
                }
                else
                {
                    IWebElement[] nextScopeSources = scopeSource.GetAll(sequalResult.ScopeSourceBy.With(searchOptions)).ToArray();

                    SearchOptions nextSearchOptions = SearchOptions.SafelyAtOnce();

                    var results = nextScopeSources.
                        Select(nextScopeSource => sequalResult.Strategy.Find(nextScopeSource, nextScopeLocateOptions, nextSearchOptions)).
                        Select(nextResult => ResolveScopeLocateResults(nextResult, nextScopeSources[0], nextSearchOptions)).
                        Where(xPathResults => xPathResults != null).
                        SelectMany(xPathResults => xPathResults).
                        ToArray();

                    if (results.Any())
                        return results;
                    else if (searchOptions.IsSafely)
                        return null;
                    else
                        throw ExceptionFactory.CreateForNoSuchElement(by: sequalResult.ScopeSourceBy);
                }
            }

            throw new ArgumentException("Unsupported ComponentScopeLocateResult type: {0}".FormatWith(result.GetType().FullName), "result");
        }
    }
}
