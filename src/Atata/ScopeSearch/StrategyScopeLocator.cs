using Humanizer;
using OpenQA.Selenium;
using System;

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

            return ResolveScopeLocateResult(result, scopeSource, searchOptions);

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

        private IWebElement ResolveScopeLocateResult(ComponentScopeLocateResult result, IWebElement scopeSource, SearchOptions searchOptions)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            MissingComponentScopeLocateResult missingResult = result as MissingComponentScopeLocateResult;
            if (missingResult != null)
                return null;

            XPathComponentScopeLocateResult xPathResult = result as XPathComponentScopeLocateResult;
            if (xPathResult != null)
                return scopeSource.Get(By.XPath(xPathResult.XPath).With(searchOptions));

            SequalComponentScopeLocateResult sequalResult = result as SequalComponentScopeLocateResult;
            if (sequalResult != null)
            {
                IWebElement nextScopeSource = sequalResult.ScopeSource ?? scopeSource.Get(sequalResult.ScopeSourceBy.With(searchOptions));
                ComponentScopeLocateResult nextResult = sequalResult.Strategy.Find(nextScopeSource, scopeLocateOptions, searchOptions);
                return ResolveScopeLocateResult(nextResult, nextScopeSource, searchOptions);
            }

            throw new ArgumentException("Unsupported ComponentScopeLocateResult type: {0}".FormatWith(result.GetType().FullName), "result");
        }
    }
}
