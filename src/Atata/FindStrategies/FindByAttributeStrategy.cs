using System.Text;

namespace Atata
{
    public class FindByAttributeStrategy : SimpleElementFindStrategy
    {
        private readonly string attributeName;

        public FindByAttributeStrategy(string attributeName)
        {
            this.attributeName = attributeName;
        }

        protected override void BuildXPath(StringBuilder builder, ElementFindOptions options)
        {
            builder.AppendFormat(
                "[{0}]",
                options.GetTermsXPathCondition("@" + attributeName));
        }
    }
}
