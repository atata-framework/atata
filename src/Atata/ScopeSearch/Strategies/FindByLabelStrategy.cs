using OpenQA.Selenium;

namespace Atata
{
    public class FindByLabelStrategy : IComponentScopeFindStrategy
    {
        public ComponentScopeFindResult Find(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
        {
            string labelXPath = new ComponentScopeXPathBuilder(options)
                .WrapWithIndex(x => x.OuterXPath._("label")[y => y.TermsConditionOfContent]);

            IWebElement label = scope.GetWithLogging(By.XPath(labelXPath).With(searchOptions).Label(options.GetTermsAsString()));

            if (label == null)
                return ComponentScopeFindResult.Missing;

            string elementId = label.GetAttribute("for");

            if (string.IsNullOrEmpty(elementId))
            {
                return new SubsequentComponentScopeFindResult(label, new FindFirstDescendantStrategy());
            }
            else if (options.Metadata.TryGet(out IdXPathForLabelAttribute idXPathForLabelAttribute))
            {
                ComponentScopeFindOptions idOptions = options.Clone();
                idOptions.Terms = new[] { idXPathForLabelAttribute.XPathFormat.FormatWith(elementId) };
                idOptions.Index = null;

                return new SubsequentComponentScopeFindResult(scope, new FindByXPathStrategy(), idOptions);
            }
            else
            {
                ComponentScopeFindOptions idOptions = options.Clone();
                idOptions.Terms = new[] { elementId };
                idOptions.Index = null;
                idOptions.Match = TermMatch.Equals;

                return new SubsequentComponentScopeFindResult(scope, new FindByIdStrategy(), idOptions);
            }
        }
    }
}
