using OpenQA.Selenium;
using System.Linq;

namespace Atata
{
    public class FindByColumnHeaderStrategy : IComponentScopeLocateStrategy
    {
        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            var headers = scope.GetAll(By.XPath("preceding::table[//th][1]//th").OfAnyVisibility().TableHeader(options.GetTermsAsString()));
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
                    throw ExceptionsFactory.CreateForNoSuchElement(options.GetTermsAsString());
            }

            return new FindByColumnIndexStrategy(columnIndex.Value).Find(scope, options, searchOptions);
        }
    }
}
