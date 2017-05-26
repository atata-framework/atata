using System.Linq;

namespace Atata
{
    public class FindByInnerXPathStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            string[] conditions = options.Terms.Length > 1
                ? options.Terms.Select(x => $"({x})").ToArray()
                : options.Terms;

            return builder.WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.JoinOr(conditions)]);
        }
    }
}
