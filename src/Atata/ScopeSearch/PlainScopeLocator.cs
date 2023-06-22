using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class PlainScopeLocator : IScopeLocator
    {
        private readonly Func<By> _byCreator;

        private By _by;

        public PlainScopeLocator(By by) =>
            _by = by.CheckNotNull(nameof(by));

        public PlainScopeLocator(Func<By> byCreator) =>
            _byCreator = byCreator.CheckNotNull(nameof(byCreator));

        private By By =>
            _by ??= _byCreator();

        public ISearchContext SearchContext { get; set; } = AtataContext.Current.Driver;

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions ??= new SearchOptions();

            return SearchContext.GetWithLogging(By.With(searchOptions));
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions ??= new SearchOptions();

            return SearchContext.GetAllWithLogging(By.With(searchOptions)).ToArray();
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions ??= new SearchOptions();

            return SearchContext.Missing(By.With(searchOptions));
        }
    }
}
