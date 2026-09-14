namespace Atata.WebDriver;

public class FindFirstDescendantOrSelfStrategy : XPathComponentScopeFindStrategy
{
    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
        builder.DescendantOrSelf.ComponentXPath;
}
