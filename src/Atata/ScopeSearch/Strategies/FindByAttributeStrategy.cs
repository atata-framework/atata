using System.Text;

namespace Atata
{
    public class FindByAttributeStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly string attributeName;

        public FindByAttributeStrategy(string attributeName)
        {
            this.attributeName = attributeName;
        }

        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            builder.AppendFormat(
                "[{0}]",
                options.GetTermsXPathCondition("@" + attributeName));
        }
    }
}
