namespace Atata;

public class FindByRelativeElementContentStrategy : XPathComponentScopeFindStrategy
{
    private readonly string _relativeElementXPath;

    public FindByRelativeElementContentStrategy(string relativeElementXPath) =>
        _relativeElementXPath = relativeElementXPath;

    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
        builder.WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y._(_relativeElementXPath)[z => z.TermsConditionOfContent]]);
}
