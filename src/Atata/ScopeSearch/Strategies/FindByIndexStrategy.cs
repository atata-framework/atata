#nullable enable

namespace Atata;

public class FindByIndexStrategy : XPathComponentScopeFindStrategy
{
    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
    {
        if (options.Index is null)
            throw new InvalidOperationException($"{nameof(FindByIndexStrategy)} can not execute because the specified Index is null.");

        return builder.WrapWithIndex(options.Index.Value, x => x.OuterXPath.ComponentXPath);
    }
}
