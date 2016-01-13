using OpenQA.Selenium;
using System.Linq;

namespace Atata
{
    public class FindByColumnHeaderStrategy : IElementFindStrategy
    {
        public ElementLocator Find(IWebElement scope, ElementFindOptions options)
        {
            var headers = scope.GetAll(By.XPath("preceding::table[//th][1]//th").OfAnyVisibility().TableHeader(options.GetQualifiersAsString()));
            var headerNamePredicate = options.Match.GetPredicate();

            int? columnIndex = headers.
                Select((x, i) => new { Text = x.Text, Index = i }).
                Where(x => options.Qualifiers.Any(qualifier => headerNamePredicate(x.Text, qualifier))).
                Select(x => (int?)x.Index).
                FirstOrDefault();

            if (columnIndex == null)
            {
                if (options.IsSafely)
                    return null;
                else
                    throw ExceptionsFactory.CreateForNoSuchElement(options.GetQualifiersAsString());
            }

            return new FindByColumnIndexStrategy(columnIndex.Value).Find(scope, options);
        }
    }
}
