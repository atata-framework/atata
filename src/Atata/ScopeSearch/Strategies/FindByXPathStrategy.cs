using System.Linq;
using System.Text;

namespace Atata
{
    public class FindByXPathStrategy : XPathComponentScopeLocateStrategy
    {
        public FindByXPathStrategy()
            : base(XPathPrefixKind.None, IndexUsage.None)
        {
        }

        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            string xPath = options.Terms.Length > 1
                ? "({0})".FormatWith(string.Join(" | ", options.Terms.Select(x => ".//" + x)))
                : ".//" + options.Terms.First();

            builder.Insert(0, "{0}{1}/descendant-or-self::".FormatWith(xPath, options.GetPositionWrappedXPathConditionOrNull()));
        }
    }
}
