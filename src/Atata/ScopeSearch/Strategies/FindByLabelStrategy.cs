using OpenQA.Selenium;

namespace Atata
{
    public class FindByLabelStrategy : IComponentScopeFindStrategy
    {
        public ComponentScopeLocateResult Find(ISearchContext scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            string labelXPath = new ComponentScopeXPathBuilder(options).
                WrapWithIndex(x => x.OuterXPath._("label")[y => y.TermsConditionOfContent]);

            IWebElement label = scope.Get(By.XPath(labelXPath).With(searchOptions).Label(options.GetTermsAsString()));

            if (label == null)
                return new MissingComponentScopeFindResult();

            string elementId = label.GetAttribute("for");
            IdXPathForLabelAttribute idXPathForLabelAttribute;

            if (string.IsNullOrEmpty(elementId))
            {
                return new SequalComponentScopeFindResult(label, new FindFirstDescendantStrategy());
            }
            else if ((idXPathForLabelAttribute = options.Metadata.Get<IdXPathForLabelAttribute>(x => x.At(AttributeLevels.Component))) != null)
            {
                ComponentScopeLocateOptions idOptions = options.Clone();
                idOptions.Terms = new[] { idXPathForLabelAttribute.XPathFormat.FormatWith(elementId) };
                idOptions.Index = null;

                return new SequalComponentScopeFindResult(scope, new FindByXPathStrategy(), idOptions);
            }
            else
            {
                ComponentScopeLocateOptions idOptions = options.Clone();
                idOptions.Terms = new[] { elementId };
                idOptions.Index = null;
                idOptions.Match = TermMatch.Equals;

                return new SequalComponentScopeFindResult(scope, new FindByIdStrategy(), idOptions);
            }
        }
    }
}
