using System.Text;

namespace Atata
{
    public class FindByFieldSetStrategy : XPathComponentScopeLocateStrategy
    {
        public FindByFieldSetStrategy()
            : base(useIndex: IndexUsage.None)
        {
        }

        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            string legendCondition = options.GetTermsXPathCondition();

            builder.Insert(0, "fieldset[legend[{0}]]{1}//".FormatWith(legendCondition, options.GetPositionWrappedXPathConditionOrNull()));
        }
    }
}
