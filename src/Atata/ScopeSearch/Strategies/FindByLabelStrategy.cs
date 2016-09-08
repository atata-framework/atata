using OpenQA.Selenium;

namespace Atata
{
    public class FindByLabelStrategy : IComponentScopeLocateStrategy
    {
        private readonly FindByIdStrategy findByIdStrategy = new FindByIdStrategy();

        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            string labelCondition = options.GetTermsXPathCondition();
            IWebElement label = scope.Get(
                By.XPath(".//label[{0}]{1}".FormatWith(labelCondition, options.GetPositionWrappedXPathConditionOrNull())).
                    With(searchOptions).
                    Label(options.GetTermsAsString()));

            if (label == null)
            {
                if (searchOptions.IsSafely)
                    return new MissingComponentScopeLocateResult();
                else
                    throw ExceptionFactory.CreateForNoSuchElement(options.GetTermsAsString(), searchContext: scope);
            }

            string elementId = label.GetAttribute("for");
            if (string.IsNullOrEmpty(elementId))
            {
                var strategy = new XPathComponentScopeLocateStrategy(useIndex: XPathComponentScopeLocateStrategy.IndexUsage.None);
                return new SequalComponentScopeLocateResult(label, strategy);
            }
            else
            {
                ComponentScopeLocateOptions idOptions = options.Clone();
                idOptions.Terms = new[] { elementId };
                idOptions.Index = null;
                idOptions.Match = TermMatch.Equals;

                return new SequalComponentScopeLocateResult(scope, findByIdStrategy, idOptions);
            }
        }
    }
}
