#nullable enable

namespace Atata;

public class FindByAttributeStrategy : XPathComponentScopeFindStrategy
{
    private readonly string _attributeName;

    public FindByAttributeStrategy(string attributeName) =>
        _attributeName = attributeName;

    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
        builder.WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.TermsConditionOf(_attributeName)]);
}
