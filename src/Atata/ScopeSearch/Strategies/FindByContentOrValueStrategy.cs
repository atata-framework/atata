using System.Text;

namespace Atata
{
    public class FindByContentOrValueStrategy : XPathComponentScopeLocateStrategy
    {
        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            builder.AppendFormat(
                "[{0} or {1}]",
                options.GetTermsXPathCondition(),
                options.GetTermsXPathCondition("@value"));
        }
    }
}
