using System.Linq;

namespace Atata
{
    public class FindByXPathStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly string[] acceptableXPathPrefixValues = { ".", "/", "(", "ancestor::", "ancestor-or-self::", "descendant::", "descendant-or-self::", "child::", "following::", "following-sibling::", "parent::", "preceding::", "preceding-sibling::", "self::" };

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            string[] conditionalXPathTerms = builder.Options.Terms.
                Where(x => (x.StartsWith("[") && x.EndsWith("]")) || x.StartsWith("@")).
                ToArray();

            string[] conditionalXPathSelectors = conditionalXPathTerms.
                Select(x => x.StartsWith("@") ? x : x.Substring(1, x.Length - 2)).
                ToArray();

            if (conditionalXPathSelectors.Length > 1)
            {
                conditionalXPathSelectors = conditionalXPathSelectors.
                    Select(x => $"({x})").
                    ToArray();
            }

            string conditionalXPath = conditionalXPathSelectors.Any()
                ? builder.WrapWithIndex(x => x.Descendant.ComponentXPath.Where(y => y.JoinOr(conditionalXPathSelectors)))
                : null;

            string[] outerXPathSelectors = builder.Options.Terms.
                Except(conditionalXPathTerms).
                Select(x => acceptableXPathPrefixValues.Any(prefix => x.StartsWith(prefix)) ? x : ".//" + x).
                ToArray();

            string outerXPath = outerXPathSelectors.Any()
                ? builder.WrapWithIndex(x => x._($"({string.Join(" | ", outerXPathSelectors)})")).DescendantOrSelf.ComponentXPath
                : null;

            if (conditionalXPath != null && outerXPath != null)
                return $"(({outerXPath}) | ({conditionalXPath}))";
            else
                return outerXPath ?? conditionalXPath;
        }
    }
}
