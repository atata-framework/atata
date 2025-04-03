#nullable enable

namespace Atata;

public class DefinedScopeLocator : IScopeLocator
{
    private readonly IWebElement? _element;

    public DefinedScopeLocator(IWebElement? element) =>
        _element = element;

    public IWebElement? GetElement(SearchOptions? searchOptions = null, string? xPathCondition = null) =>
        _element;

    public IWebElement[] GetElements(SearchOptions? searchOptions = null, string? xPathCondition = null) =>
        _element is null
            ? []
            : [_element];

    public bool IsMissing(SearchOptions? searchOptions = null, string? xPathCondition = null) =>
        _element is null;
}
