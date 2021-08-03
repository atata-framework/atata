using System;
using System.Linq;

namespace Atata
{
    public class FindByXPathStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly string[] _acceptableXPathPrefixValues =
        {
            ".",
            "/",
            "(",
            "ancestor::",
            "ancestor-or-self::",
            "descendant::",
            "descendant-or-self::",
            "child::",
            "following::",
            "following-sibling::",
            "parent::",
            "preceding::",
            "preceding-sibling::",
            "self::"
        };

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            string[] conditionalXPathTerms = builder.Options.Terms.
                Where(x => (x[0] == '[' && x[x.Length - 1] == ']') || x[0] == '@').
                ToArray();

            string[] conditionalXPathSelectors = conditionalXPathTerms.
                Select(x => x[0] == '@' ? x : x.Substring(1, x.Length - 2)).
                ToArray();

            if (conditionalXPathSelectors.Length > 1)
            {
                conditionalXPathSelectors = conditionalXPathSelectors.
                    Select(x => $"({x})").
                    ToArray();
            }

            string conditionalXPath = conditionalXPathSelectors.Any()
                ? builder.WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.JoinOr(conditionalXPathSelectors)])
                : null;

            string[] completeXPathSelectors = builder.Options.Terms.
                Except(conditionalXPathTerms).
                Select(x =>
                    _acceptableXPathPrefixValues.Any(prefix => x.StartsWith(prefix, StringComparison.Ordinal))
                        ? (options.OuterXPath?.Append(x) ?? x)
                        : ((options.OuterXPath ?? ".//") + x)).
                ToArray();

            string completeXPath = completeXPathSelectors.Any()
                ? builder.WrapWithIndex(x => x._($"({string.Join(" | ", completeXPathSelectors)})")).DescendantOrSelf.ComponentXPath
                : null;

            return conditionalXPath != null && completeXPath != null
                ? $"(({completeXPath}) | ({conditionalXPath}))"
                : completeXPath ?? conditionalXPath;
        }
    }
}
