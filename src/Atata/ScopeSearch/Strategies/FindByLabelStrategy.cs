using OpenQA.Selenium;

namespace Atata
{
    public class FindByLabelStrategy : IComponentScopeFindStrategy
    {
        public ComponentScopeFindResult Find(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions) =>
            scope is IWebDriver || scope is IWrapsDriver
                ? FindUsingOneQuery(scope, options, searchOptions)
                : FindLabelThenComponent(scope, options, searchOptions);

        private static ComponentScopeFindResult FindUsingOneQuery(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
        {
            string xPath = new ComponentScopeXPathBuilder(options)
                    .Wrap(j => j
                        .OuterXPath.Any[x => x._("@id = ").WrapWithIndex(l => l._("//label")[lc => lc.TermsConditionOfContent])._("/@for")].DescendantOrSelf.ComponentXPath
                        ._(" | ").WrapWithIndex(l => l.OuterXPath._("label")[lc => lc.TermsConditionOfContent]).DescendantOrSelf.ComponentXPath);

            return new XPathComponentScopeFindResult(xPath, scope, searchOptions);
        }

        private static ComponentScopeFindResult FindLabelThenComponent(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
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
