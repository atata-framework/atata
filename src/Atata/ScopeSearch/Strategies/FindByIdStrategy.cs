using System.Linq;

namespace Atata
{
    public class FindByIdStrategy : XPathComponentScopeLocateStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.Descendant.Any.Where(y =>
                    string.IsNullOrWhiteSpace(options.IdXPathFormat)
                        ? y.TermsConditionOf("id")
                        : y.JoinOr(options.Terms.Select(term => options.IdXPathFormat.FormatWith(term))))).
                DescendantOrSelf.ComponentXPath;
        }
    }
}
