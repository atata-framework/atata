namespace Atata;

public class FindByInnerXPathStrategy : XPathComponentScopeFindStrategy
{
    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
    {
        string[] conditions = options.Terms.Length > 1
            ? [.. options.Terms.Select(x => $"({x})")]
            : options.Terms;

        return builder.WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.JoinOr(conditions)]);
    }
}
