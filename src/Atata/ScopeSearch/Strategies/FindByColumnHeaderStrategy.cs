using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class FindByColumnHeaderStrategy : IComponentScopeLocateStrategy
    {
        private readonly string headerXPath;

        public FindByColumnHeaderStrategy()
            : this("(ancestor::table)[1]//th")
        {
        }

        public FindByColumnHeaderStrategy(string headerXPath)
        {
            this.headerXPath = headerXPath;
        }

        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            var headers = scope.GetAll(By.XPath(headerXPath).With(searchOptions).OfAnyVisibility());
            var headerNamePredicate = options.Match.GetPredicate();

            int? columnIndex = headers.
                Select((x, i) => new { Text = x.Text, Index = i }).
                Where(x => options.Terms.Any(term => headerNamePredicate(x.Text, term))).
                Select(x => (int?)x.Index).
                FirstOrDefault();

            if (columnIndex == null)
            {
                if (searchOptions.IsSafely)
                    return new MissingComponentScopeLocateResult();
                else
                    throw ExceptionFactory.CreateForNoSuchElement(options.GetTermsAsString(), searchContext: scope);
            }

            return new FindByColumnIndexStrategy(columnIndex.Value).Find(scope, options, searchOptions);
        }
    }
}
