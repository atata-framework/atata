using System.Linq;

namespace Atata
{
    public class FindByClassStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            string classCondition = GetClassCondition(options);

            return builder.
                WrapWithIndex(x => x.OuterXPath.Any[classCondition]).
                DescendantOrSelf.ComponentXPath;
        }

        private static string GetClassCondition(ComponentScopeFindOptions options)
        {
            string conditionFormat = "contains(concat(' ', normalize-space(@class), ' '), ' {0} ')";

            var conditionOrParts = options.Terms.
                Select(t => t.Split(' ').Where(qp => !string.IsNullOrWhiteSpace(qp)).ToArray()).
                Where(qps => qps.Any()).
                Select(qps => string.Join(" and ", qps.Select(qp => conditionFormat.FormatWith(qp)))).
                ToArray();

            return conditionOrParts.Length == 1
                ? conditionOrParts.First()
                : string.Join(" or ", conditionOrParts.Select(x => "({0})".FormatWith(x)));
        }
    }
}
