using System.Linq;

namespace Atata
{
    public class FindByXPathStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly string[] xPathPrefixValues = { "./", "/", "descendant", "child" };

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            string[] conditionalXPathSelectors = builder.Options.Terms.
                Where(x => x.StartsWith("[") && x.EndsWith("]")).
                Select(x => x.Substring(1, x.Length - 2)).
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
                Where(x => !(x.StartsWith("[") && x.EndsWith("]"))).
                Select(x => xPathPrefixValues.Any(prefix => x.StartsWith(prefix)) ? x : ".//" + x).
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
