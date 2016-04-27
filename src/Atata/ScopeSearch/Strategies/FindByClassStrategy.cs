using System.Linq;
using System.Text;

namespace Atata
{
    public class FindByClassStrategy : XPathComponentScopeLocateStrategy
    {
        public FindByClassStrategy()
            : base(useIndex: IndexUsage.None)
        {
        }

        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            string classCondition = GetClassCondition(options);

            builder.Insert(0, "*[{0}]{1}/descendant-or-self::".FormatWith(classCondition, options.GetPositionWrappedXPathConditionOrNull()));
        }

        private string GetClassCondition(ComponentScopeLocateOptions options)
        {
            string conditionFormat = "contains(concat(' ', normalize-space(@class), ' '), ' {0} ')";

            var conditionOrParts = options.Terms.
                Select(t => t.Split(' ').Where(qp => !string.IsNullOrWhiteSpace(qp)).ToArray()).
                Where(qps => qps.Any()).
                Select(qps => string.Join(" and ", qps.Select(qp => conditionFormat.FormatWith(qp)))).
                ToArray();

            if (conditionOrParts.Count() == 1)
                return conditionOrParts.First();
            else
                return string.Join(" or ", conditionOrParts.Select(x => "({0})".FormatWith(x)));
        }
    }
}
