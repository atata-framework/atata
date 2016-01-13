using Humanizer;
using System.Text;

namespace Atata
{
    public class FindByNameStrategy : SimpleElementFindStrategy
    {
        public FindByNameStrategy()
            : base(applyIndex: false)
        {
        }

        protected override void BuildXPath(StringBuilder builder, ElementFindOptions options)
        {
            string nameCondition = options.GetQualifiersXPathCondition("@name");

            builder.Insert(0, "*[{0}]{1}/descendant-or-self::".FormatWith(nameCondition, options.GetPositionWrappedXPathCondition()));
        }
    }
}
