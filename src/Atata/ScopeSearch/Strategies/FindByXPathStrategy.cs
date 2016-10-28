using System.Linq;

namespace Atata
{
    public class FindByXPathStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            string baseXPath = string.Join(" | ", builder.Options.Terms.Select(x => ".//" + x));

            return builder.WrapWithIndex(x => x._(baseXPath)).DescendantOrSelf.ComponentXPath;
        }
    }
}
