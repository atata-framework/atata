using Humanizer;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace Atata
{
    public class StrategyScopeLocator : IScopeLocator
    {
        private readonly UIComponent component;
        private readonly IComponentScopeLocateStrategy strategy;
        private readonly ComponentScopeLocateOptions scopeLocateOptions;

        public StrategyScopeLocator(UIComponent component, IComponentScopeLocateStrategy elementLocator, ComponentScopeLocateOptions scopeLocateOptions)
        {
            this.component = component;
            this.strategy = elementLocator;
            this.scopeLocateOptions = scopeLocateOptions;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string extraXPath = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            IWebElement scopeSource = component.ScopeSource.GetScopeElement(component.Parent);

            if (scopeSource == null && searchOptions.IsSafely)
                return null;

            ComponentScopeLocateResult result = strategy.Find(scopeSource, scopeLocateOptions, searchOptions);

            XPathComponentScopeLocateResult[] xPathResults = ResolveScopeLocateResult(result, scopeSource, searchOptions);
            if (xPathResults.Any())
            {
                return xPathResults.Select(x => x.Get(extraXPath)).FirstOrDefault();
            }
            else
            {
                return null;
            }

            ////if (extraXPath == null)
            ////{
            ////    By scopeBy = elementLocator.Find(scopeSource, findOptions, searchOptions);
            ////    return scopeSource.Get(scopeBy.Named(findOptions.ComponentName).With(searchOptions));
            ////}
            ////else if (elementLocator is IXPathElementFindStrategy)
            ////{
            ////    IXPathElementFindStrategy xPathStrategy = (IXPathElementFindStrategy)elementLocator;
            ////    string baseXPath = xPathStrategy.BuildXPath(findOptions);
            ////    string completeXPath = string.Format("{0}//{1}", baseXPath, extraXPath);
            ////    return scopeSource.Get(By.XPath(completeXPath).Named(findOptions.ComponentName).With(searchOptions));
            ////}
            ////else
            ////{
            ////    return null;
            ////}
        }

        private XPathComponentScopeLocateResult[] ResolveScopeLocateResult(ComponentScopeLocateResult result, IWebElement scopeSource, SearchOptions searchOptions)
        {
            if (result == null)
                throw new ArgumentNullException("result");

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
                    return ResolveScopeLocateResult(nextResult, sequalResult.ScopeSource, searchOptions);
                }
                else
                {
                    IWebElement[] nextScopeSources = scopeSource.GetAll(sequalResult.ScopeSourceBy.With(searchOptions)).ToArray();

                    SearchOptions nextSearchOptions = new SearchOptions
                    {
                        IsSafely = true,
                        Timeout = TimeSpan.Zero,
                        Visibility = searchOptions.Visibility
                    };

                    var results = nextScopeSources.
                        Select(nextScopeSource => sequalResult.Strategy.Find(nextScopeSource, nextScopeLocateOptions, nextSearchOptions)).
                        Select(nextResult => ResolveScopeLocateResult(nextResult, nextScopeSources[0], nextSearchOptions)).
                        Where(xPathResults => xPathResults != null).
                        SelectMany(xPathResults => xPathResults).
                        ToArray();

                    if (results.Any())
                        return results;
                    else if (searchOptions.IsSafely)
                        return null;
                    else
                        ExceptionsFactory.CreateForNoSuchElement(by: sequalResult.ScopeSourceBy);
                }
            }

            throw new ArgumentException("Unsupported ComponentScopeLocateResult type: {0}".FormatWith(result.GetType().FullName), "result");
        }
    }
}
