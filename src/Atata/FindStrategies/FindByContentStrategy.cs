using System.Text;

namespace Atata
{
    public class FindByContentStrategy : XPathElementFindStrategy
    {
        protected override void BuildXPath(StringBuilder builder, ElementFindOptions options)
        {
            builder.AppendFormat(
                "[{0}]",
                options.GetTermsXPathCondition());
        }
    }
}
