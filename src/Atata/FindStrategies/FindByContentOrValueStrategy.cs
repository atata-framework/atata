using System.Text;

namespace Atata
{
    public class FindByContentOrValueStrategy : XPathElementFindStrategy
    {
        protected override void BuildXPath(StringBuilder builder, ElementFindOptions options)
        {
            builder.AppendFormat(
                "[{0} or {1}]",
                options.GetTermsXPathCondition(),
                options.GetTermsXPathCondition("@value"));
        }
    }
}
