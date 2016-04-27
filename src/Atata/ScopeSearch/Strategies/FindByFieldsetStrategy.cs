using System.Text;

namespace Atata
{
    public class FindByFieldsetStrategy : XPathComponentScopeLocateStrategy
    {
        public FindByFieldsetStrategy()
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
