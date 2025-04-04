#nullable enable

namespace Atata;

public interface ILayerScopeContextResolver
{
    string DefaultOuterXPath { get; }

    ISearchContext Resolve(IWebElement element, WebDriverSession session);
}
