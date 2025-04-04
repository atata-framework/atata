namespace Atata;

public class FindByXPathStrategy : XPathComponentScopeFindStrategy
{
    private readonly string[] _acceptableXPathPrefixValues =
    [
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
    ];

    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
    {
        string[] conditionalXPathTerms = builder.Options.Terms
            .Where(x => (x[0] == '[' && x[^1] == ']') || x[0] == '@')
            .ToArray();

        string[] conditionalXPathSelectors = conditionalXPathTerms
            .Select(x => x[0] == '@' ? x : x[1..^1])
            .ToArray();

        if (conditionalXPathSelectors.Length > 1)
        {
            conditionalXPathSelectors = conditionalXPathSelectors
                .Select(x => $"({x})")
                .ToArray();
        }

        string? conditionalXPath = conditionalXPathSelectors.Length > 0
            ? builder.WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.JoinOr(conditionalXPathSelectors)])
            : null;

        string[] completeXPathSelectors = builder.Options.Terms
            .Except(conditionalXPathTerms)
            .Select(x =>
                _acceptableXPathPrefixValues.Any(prefix => x.StartsWith(prefix, StringComparison.Ordinal))
                    ? (options.OuterXPath?.Append(x) ?? x)
                    : ((options.OuterXPath ?? ".//") + x))
            .ToArray();

        string? completeXPath = completeXPathSelectors.Length > 0
            ? builder.WrapWithIndex(x => x._($"({string.Join(" | ", completeXPathSelectors)})")).DescendantOrSelf.ComponentXPath
            : null;

        return conditionalXPath is not null && completeXPath is not null
            ? $"(({completeXPath}) | ({conditionalXPath}))"
            : completeXPath ?? conditionalXPath!;
    }
}
