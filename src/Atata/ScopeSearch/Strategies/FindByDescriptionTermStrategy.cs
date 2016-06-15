using System.Text;

namespace Atata
{
    public class FindByDescriptionTermStrategy : XPathComponentScopeLocateStrategy
    {
        public FindByDescriptionTermStrategy()
            : base(useIndex: IndexUsage.None)
        {
        }

        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            string termCondition = options.GetTermsXPathCondition();

            builder.Insert(0, "dl/dt[{0}]{1}/following-sibling::dd/descendant-or-self::".FormatWith(termCondition, options.GetPositionWrappedXPathConditionOrNull()));
        }
    }
}
