using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class PlainScopeLocator : IScopeLocator
    {
        private readonly Func<By> byCreator;

        private By by;

        public PlainScopeLocator(By by)
        {
            by.CheckNotNull(nameof(by));

            this.by = by;
        }

        public PlainScopeLocator(Func<By> byCreator)
        {
            byCreator.CheckNotNull(nameof(byCreator));

            this.byCreator = byCreator;
        }

        private By By
        {
            get { return by ?? (by = byCreator()); }
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            return ATContext.Current.Driver.Get(By.With(searchOptions));
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            return ATContext.Current.Driver.GetAll(By.With(searchOptions)).ToArray();
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            return ATContext.Current.Driver.Missing(By.With(searchOptions));
        }
    }
}
