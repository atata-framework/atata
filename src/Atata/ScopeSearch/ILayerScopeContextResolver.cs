using OpenQA.Selenium;

namespace Atata
{
    public interface ILayerScopeContextResolver
    {
        ISearchContext Resolve(IWebElement element);
    }
}
