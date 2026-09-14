namespace Atata.WebDriver;

public interface ILayerScopeContextResolver
{
    string DefaultOuterXPath { get; }

    ISearchContext Resolve(IWebElement element, WebDriverSession session);
}
