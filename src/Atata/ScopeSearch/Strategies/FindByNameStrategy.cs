#nullable enable

namespace Atata;

public class FindByNameStrategy : XPathComponentScopeFindStrategy
{
    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
        builder
            .WrapWithIndex(x => x.OuterXPath.Any[y => y.TermsConditionOf("name")])
            .DescendantOrSelf.ComponentXPath;
}
