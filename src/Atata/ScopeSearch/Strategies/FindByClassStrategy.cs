namespace Atata;

public class FindByClassStrategy : XPathComponentScopeFindStrategy
{
    private const string ClassConditionFormat = "contains(concat(' ', normalize-space(@class), ' '), ' {0} ')";

    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
    {
        string classCondition = GetClassCondition(options);

        return builder
            .WrapWithIndex(x => x.OuterXPath.Any[classCondition])
            .DescendantOrSelf.ComponentXPath;
    }

    private static string GetClassCondition(ComponentScopeFindOptions options)
    {
        var conditionOrParts = options.Terms
            .Select(t => t.Split(' ').Where(qp => !string.IsNullOrWhiteSpace(qp)).ToArray())
            .Where(qps => qps.Length > 0)
            .Select(qps => string.Join(" and ", qps.Select(qp => ClassConditionFormat.FormatWith(qp))))
            .ToArray();

        return conditionOrParts.Length == 1
            ? conditionOrParts[0]
            : string.Join(" or ", conditionOrParts.Select(x => "({0})".FormatWith(x)));
    }
}
