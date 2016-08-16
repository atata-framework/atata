using System.Linq;
using System.Text;

namespace Atata
{
    public class FindByInnerXPathStrategy : XPathComponentScopeLocateStrategy
    {
        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            string xPath = options.Terms.Length > 1
                ? "({0})".FormatWith(string.Join(" | ", options.Terms))
                : options.Terms.First();

            builder.AppendFormat("[{0}]".FormatWith(xPath));
        }
    }
}
