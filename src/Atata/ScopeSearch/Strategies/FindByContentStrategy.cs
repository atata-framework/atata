using System.Text;

namespace Atata
{
    public class FindByContentStrategy : XPathComponentScopeLocateStrategy
    {
        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            builder.AppendFormat(
                "[{0}]",
                options.GetTermsXPathCondition());
        }
    }
}
