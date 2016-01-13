using Humanizer;
using System.Linq;
using System.Text;

namespace Atata
{
    public class FindByXPathStrategy : SimpleElementFindStrategy
    {
        public FindByXPathStrategy()
            : base(XPathPrefixKind.None, applyIndex: false)
        {
        }

        protected override void BuildXPath(StringBuilder builder, ElementFindOptions options)
        {
            string xPath = options.Qualifiers.Length > 1
                ? "({0})".FormatWith(string.Join(" | ", options.Qualifiers.Select(x => ".//" + x)))
                : ".//" + options.Qualifiers.First();

            builder.Insert(0, "{0}{1}/descendant-or-self::".FormatWith(xPath, options.GetPositionWrappedXPathCondition()));
        }
    }
}
