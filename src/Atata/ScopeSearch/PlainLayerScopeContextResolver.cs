namespace Atata;

public class PlainLayerScopeContextResolver : ILayerScopeContextResolver
{
    public PlainLayerScopeContextResolver(string defaultOuterXPath) =>
        DefaultOuterXPath = defaultOuterXPath;

    public string DefaultOuterXPath { get; }

    public ISearchContext Resolve(IWebElement element, WebDriverSession session) =>
        element;
}
