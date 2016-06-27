using System.Linq;
using System.Text;

namespace Atata
{
    public class FindByIdStrategy : XPathComponentScopeLocateStrategy
    {
        public FindByIdStrategy()
            : base(useIndex: IndexUsage.None)
        {
        }

        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            string idCondition = string.IsNullOrWhiteSpace(options.IdXPathFormat)
                ? options.GetTermsXPathCondition("@id")
                : string.Join(" or ", options.Terms.Select(x => options.IdXPathFormat.FormatWith(x)));

            builder.Insert(0, "*[{0}]{1}/descendant-or-self::".FormatWith(idCondition, options.GetPositionWrappedXPathConditionOrNull()));
        }
    }
}
