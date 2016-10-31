using System.Linq;

namespace Atata
{
    public class FindByIdStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            string idCondition = string.IsNullOrWhiteSpace(options.IdXPathFormat)
                ? builder.TermsConditionOf("id")
                : builder.JoinOr(options.Terms.Select(term => options.IdXPathFormat.FormatWith(term)));

            return builder.WrapWithIndex(x => x.Descendant.Any[idCondition]).DescendantOrSelf.ComponentXPath;
        }
    }
}
