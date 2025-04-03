#nullable enable

namespace Atata;

public class FindByLabelStrategy : IComponentScopeFindStrategy
{
    public ComponentScopeFindResult Find(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions) =>
        scope is IWebDriver or IWrapsDriver && !options.Metadata.Contains<IdXPathForLabelAttribute>()
            ? FindUsingOneQuery(scope, options, searchOptions)
            : FindLabelThenComponent(scope, options, searchOptions);

    private static XPathComponentScopeFindResult FindUsingOneQuery(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
    {
        string xPath = new ComponentScopeXPathBuilder(options)
            .Wrap(j => j
                .OuterXPath.Any[x => x._("@id = ").WrapWithIndex(l => l._("//label")[lc => lc.TermsConditionOfContent])._("/@for")].DescendantOrSelf.ComponentXPath
                ._(" | ").WrapWithIndex(l => l.OuterXPath._("label")[lc => lc.TermsConditionOfContent]).DescendantOrSelf.ComponentXPath);

        return new(xPath, scope, searchOptions, options.Component);
    }

    private static ComponentScopeFindResult FindLabelThenComponent(ISearchContext scope, ComponentScopeFindOptions options, SearchOptions searchOptions)
    {
        string labelXPath = new ComponentScopeXPathBuilder(options)
            .WrapWithIndex(x => x.OuterXPath._("label")[y => y.TermsConditionOfContent]);

        IWebElement? label = scope.GetWithLogging(
            options.Component.Log,
            By.XPath(labelXPath).With(searchOptions).Label(options.GetTermsAsString()));

        if (label is null)
            return ComponentScopeFindResult.Missing;

        string? elementId = label.GetAttribute("for");

        if (elementId is null or [])
        {
            return new SubsequentComponentScopeFindResult(label, new FindFirstDescendantStrategy());
        }
        else if (options.Metadata.TryGet(out IdXPathForLabelAttribute idXPathForLabelAttribute) && idXPathForLabelAttribute.XPathFormat?.Length > 0)
        {
            ComponentScopeFindOptions idOptions = options.Clone();
            idOptions.Terms = [idXPathForLabelAttribute.XPathFormat.FormatWith(elementId)];
            idOptions.Index = null;

            return new SubsequentComponentScopeFindResult(scope, new FindByXPathStrategy(), idOptions);
        }
        else
        {
            ComponentScopeFindOptions idOptions = options.Clone();
            idOptions.Terms = [elementId];
            idOptions.Index = null;
            idOptions.Match = TermMatch.Equals;

            return new SubsequentComponentScopeFindResult(scope, new FindByIdStrategy(), idOptions);
        }
    }
}
